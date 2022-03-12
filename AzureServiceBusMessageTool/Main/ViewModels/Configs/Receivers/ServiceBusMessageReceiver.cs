﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Core.Maybe;
using Main.Application;
using Main.Application.Logging;
using Main.ExceptionHandling;
using Main.Models;
using Main.Utils;

namespace Main.ViewModels.Configs.Receivers;

public class ServiceBusMessageReceiver : IServiceBusMessageReceiver
{
    private readonly IServiceBusHelperLogger _logger;
    private readonly IReceiverSettingsValidator _receiversSettingsValidator;
    private CancellationTokenSource _cancellationTokenSource;
    private ServiceBusReceiverSettings _config;
    private ReceiverCallbacks _callbacks;
    private ServiceBusClient _client;
    private StopReason _stopReason = StopReason.Intentional;
    private ReceivedMessageFormatter _msgFormatter;

    public ServiceBusMessageReceiver(IServiceBusHelperLogger logger,
        IReceiverSettingsValidator receiversSettingsValidator)
    {
        _logger = logger;
        _receiversSettingsValidator = receiversSettingsValidator;
        _msgFormatter = new ReceivedMessageFormatter();
    }

    private enum StopReason
    {
        Unexpected = 0,
        Intentional
    }

    public void Start(
        ServiceBusReceiverSettings config,
        ReceiverCallbacks callbacks)
    {
        _config = config;
        _callbacks = callbacks;
        _stopReason = StopReason.Unexpected;
        try
        {
            if (_cancellationTokenSource != null)
            {
                throw new ServiceBusHelperException($"InternalError: Receiver '{_config.ConfigName}' already started");
            }

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await ValidateConfigOrThrow(_config, token);
                    _client = new ServiceBusClient(_config.ConnectionString);

                    var serviceBusReceiverOptions = GetServiceBusReceiverOptionsBasedOnConfig();


                    await ReceiveUntilCancelledOrStopped(serviceBusReceiverOptions, token);
                }
                catch (TaskCanceledException)
                {
                    if (_stopReason == StopReason.Intentional)
                    {
                        _logger.LogInfo($"Receiver '{_config.ConfigName}' was stopped manually.");
                    }
                    else
                    {
                        _logger.LogError($"Receiver '{_config.ConfigName}' was stopped unexpectedly");
                    }

                    StopReceiver();
                }
                catch (Exception e)
                {
                    _callbacks.OnReceiverFailure.Invoke(e);
                    _logger.LogException(e);
                    StopReceiver();
                }
            }, TaskCreationOptions.LongRunning);
        }
        catch (Exception e)
        {
            StopReceiver();
            callbacks.OnReceiverFailure.Invoke(e);
            _logger.LogException($"Could not start receiving for receiver '{_config.ConfigName}' for messages because of error.", e);
        }
    }

    private async Task ReceiveUntilCancelledOrStopped(ServiceBusReceiverOptions serviceBusReceiverOptions,
        CancellationToken token)
    {
        await using var receiver = _client.CreateReceiver(
            _config.TopicName,
            _config.SubscriptionName,
            serviceBusReceiverOptions);

        _logger.LogInfo($"Receiver '{_config.ConfigName}' started.");
        _callbacks.OnReceiverStarted.Invoke();
        while (!token.IsCancellationRequested)
        {
            var message = await receiver.ReceiveMessageAsync(StaticConfig.MessageReceiverReceiveTimeout, token);

            if (message != null)
            {
                var receivedMessage = new ReceivedMessage
                {
                    Body = _msgFormatter.Format(message)
                };
                _callbacks.OnMessageReceive(receivedMessage);
                await FinalizeMessageReceiveForPickLockMode(message, receiver, token);
            }

            await Task.Delay(_config.MessageReceiveDelayPeriod);
        }
    }

    private void StopReceiver()
    {
        if (_stopReason == StopReason.Intentional)
        {
            _callbacks.OnReceiverStop.Invoke();
        }

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
        _client?.DisposeAsync();
    }

    private async Task ValidateConfigOrThrow(ServiceBusReceiverSettings config, CancellationToken token)
    {
        _callbacks.OnReceiverInitializing.Invoke();
        var validationResult = await _receiversSettingsValidator.Validate(config, token);
        if (validationResult.IsSomething())
        {
            throw new InvalidConfigurationException(
                $"Receiver '{_config.ConfigName}' configuration is invalid: '{validationResult.Value().ErrorMsg}'");
        }
    }

    private async Task FinalizeMessageReceiveForPickLockMode(
        ServiceBusReceivedMessage message,
        ServiceBusReceiver serviceBusReceiver,
        CancellationToken cancellationToken)
    {
        if (serviceBusReceiver.ReceiveMode != ServiceBusReceiveMode.PeekLock)
        {
            return;
        }

        var onReceiveAction = _config.OnMessageReceiveEnumAction;
        if (onReceiveAction == OnMessageReceiveEnumAction.Abandon)
        {
            await AbandonMessage(message, serviceBusReceiver, cancellationToken);
        }
        else if (onReceiveAction == OnMessageReceiveEnumAction.Complete)
        {
            await serviceBusReceiver.CompleteMessageAsync(message, cancellationToken);
        }
        else if (onReceiveAction == OnMessageReceiveEnumAction.MoveToDeadLetter)
        {
            await DeadLetterMessage(message, serviceBusReceiver, cancellationToken);
        }
        else
        {
            throw new AsbMessageToolException($"Internal error: unhandled enumeration '{onReceiveAction}'");
        }
    }

    private async Task DeadLetterMessage(
        ServiceBusReceivedMessage message,
        ServiceBusReceiver serviceBusReceiver,
        CancellationToken cancellationToken)
    {
        switch (_config.DeadLetterMessageFieldsOverrideType)
        {
            case DeadLetterMessageFieldsOverrideEnumType.OverrideDeadLetterErrorRelatedFields:

                var deadLetterMessageFields = _config.DeadLetterMessageFields;
                await serviceBusReceiver.DeadLetterMessageAsync(message,
                    deadLetterMessageFields.DeadLetterReason.ValueIfEnabledOrNull,
                    deadLetterMessageFields.DeadLetterErrorDescription.ValueIfEnabledOrNull,
                    cancellationToken);
                break;

            case DeadLetterMessageFieldsOverrideEnumType.OverrideApplicationPropertiesFields:
                var asPropertyDictionary = _config.DeadLetterMessageOverriddenApplicationProperties.AsPropertyDictionary();
                await serviceBusReceiver.DeadLetterMessageAsync(message, asPropertyDictionary, cancellationToken);
                break;

            default:
                throw new AsbMessageToolException($"Internal error: unhandled enumeration '{_config.DeadLetterMessageFieldsOverrideType}'");
        }
    }


    private async Task AbandonMessage(
        ServiceBusReceivedMessage message,
        ServiceBusReceiver serviceBusReceiver,
        CancellationToken cancellationToken)
    {
        await serviceBusReceiver.AbandonMessageAsync(message, _config.AbandonMessageOverriddenApplicationProperties.AsPropertyDictionary(), cancellationToken);
    }

    private ServiceBusReceiverOptions GetServiceBusReceiverOptionsBasedOnConfig()
    {
        if (_config.IsDeadLetterQueue)
        {
            return new ServiceBusReceiverOptions
            {
                SubQueue = SubQueue.DeadLetter,
                ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete,
            };
        }

        return new ServiceBusReceiverOptions
        {
            SubQueue = SubQueue.None,
            ReceiveMode = ServiceBusReceiveMode.PeekLock,
        };
    }

    public void Stop()
    {
        _stopReason = StopReason.Intentional;
        StopReceiver();
    }
}

public class ReceivedMessage
{
    public string Body { get; init; }
}

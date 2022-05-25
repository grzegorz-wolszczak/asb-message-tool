using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ASBMessageTool.Application;
using ASBMessageTool.Application.Config;
using ASBMessageTool.Application.Logging;
using ASBMessageTool.Model;
using ASBMessageTool.ReceivingMessages.Code;
using Azure.Messaging.ServiceBus;
using Core.Maybe;

namespace ASBMessageTool.PeekingMessages.Code;

public class ServiceBusMessagePeekerFactory
{
    private readonly IServiceBusHelperLogger _logger;
    private readonly IPeekerSettingsValidator _peekerSettingsValidator;

    public ServiceBusMessagePeekerFactory(IServiceBusHelperLogger logger, IPeekerSettingsValidator peekerSettingsValidator)
    {
        _logger = logger;
        _peekerSettingsValidator = peekerSettingsValidator;
    }

    public IServiceBusMessagePeeker Create()
    {
        return new ServiceBusMessageMessagePeeker(_peekerSettingsValidator, _logger);
    }
}

public interface IServiceBusMessagePeeker
{
    void Start(ServiceBusPeekerSettings config, PeekerCallbacks callbacks);
}

public class ServiceBusMessageMessagePeeker : IServiceBusMessagePeeker
{
    
    private ServiceBusClient _client;
    
    private readonly IPeekerSettingsValidator _peekerSettingsValidator;
    private readonly IServiceBusHelperLogger _logger;
    private ServiceBusPeekerSettings _config;
    private PeekerCallbacks _callbacks;

    public ServiceBusMessageMessagePeeker(
        IPeekerSettingsValidator peekerSettingsValidator, 
        IServiceBusHelperLogger logger)
    {
        _peekerSettingsValidator = peekerSettingsValidator;
        _logger = logger;
    }

    public void Start(ServiceBusPeekerSettings config, PeekerCallbacks callbacks)
    {
        _config = config;
        _callbacks = callbacks;

        try
        {
            var token = CancellationToken.None;

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await ValidateConfigOrThrow(_config, token);
                    _client = new ServiceBusClient(_config.ConnectionString);
                    var serviceBusReceiverOptions = GetServiceBusPeekerOptionsBasedOnConfig();
                    await ReceiveUntilFinishedPeekingOrError(serviceBusReceiverOptions, token);
                    _callbacks.OnPeekerFinished.Invoke();
                }
                catch (TaskCanceledException e)
                {
                    _logger.LogError($"Peeker '{_config.ConfigName}' was stopped unexpectedly (task cancelled)");
                    _callbacks.OnPeekerFailure.Invoke(e);
                }
                catch (Exception e)
                {
                    _callbacks.OnPeekerFailure.Invoke(e);
                    _logger.LogException($"Error happened while peeking messages for peeker '{_config.ConfigName}'", e);
                }
            }, TaskCreationOptions.LongRunning);
        }
        catch (Exception e)
        {
            callbacks.OnPeekerFailure.Invoke(e);
            _logger.LogException($"Could not start peeking messages for peeker '{_config.ConfigName}' because of error.", e);
        }
    }

    private async Task ReceiveUntilFinishedPeekingOrError(ServiceBusReceiverOptions serviceBusReceiverOptions,
        CancellationToken token)
    {
        await using var receiver = CreateReceiverForReceiveServiceType(serviceBusReceiverOptions);

        _logger.LogInfo($"Receiver '{_config.ConfigName}' started.");
        _callbacks.OnPeekerStarted.Invoke();

        var messages = await receiver.PeekMessagesAsync(int.MaxValue, 0, token);
        List<PeekedMessage> allMessagesPeeked = new List<PeekedMessage>();
        if (messages is { Count: > 0 })
        {
            allMessagesPeeked = messages.Select(x => new PeekedMessage() { OriginalMessage = x }).ToList();
        }

        _callbacks.OnAllMessagesPeeked.Invoke(allMessagesPeeked);
    }


    private ServiceBusReceiver CreateReceiverForReceiveServiceType(ServiceBusReceiverOptions serviceBusReceiverOptions)
    {
        if (_config.ReceiverDataSourceType == ReceiverDataSourceType.Topic)
        {
            return _client.CreateReceiver(
                _config.TopicName,
                _config.SubscriptionName,
                serviceBusReceiverOptions);
        }

        if (_config.ReceiverDataSourceType == ReceiverDataSourceType.Queue)
        {
            return _client.CreateReceiver(_config.ReceiverQueueName, serviceBusReceiverOptions);
        }

        throw new AsbMessageToolException($"InternalError: Unhandled enum :'{_config.ReceiverDataSourceType}'");
    }


    private async Task ValidateConfigOrThrow(ServiceBusPeekerSettings config, CancellationToken token)
    {
        _callbacks.OnPeekerInitializing.Invoke();
        var validationResult = await _peekerSettingsValidator.Validate(config, token);
        if (validationResult.IsSomething())
        {
            throw new InvalidConfigurationException(
                $"Peeker '{_config.ConfigName}' configuration is invalid: '{validationResult.Value().ErrorMsg}'");
        }
    }

    private ServiceBusReceiverOptions GetServiceBusPeekerOptionsBasedOnConfig()
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
}

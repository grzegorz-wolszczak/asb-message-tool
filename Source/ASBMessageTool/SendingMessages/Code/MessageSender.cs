using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ASBMessageTool.Application.Logging;
using ASBMessageTool.Model;
using Azure.Messaging.ServiceBus;
using Core.Maybe;

namespace ASBMessageTool.SendingMessages.Code;

public class MessageSender : IMessageSender
{
    private readonly Dictionary<string /* connection string */, ServiceBusClient> _serviceBusClientsCache = new();
    private readonly Dictionary<(string /* connection string */, string /*topicName*/), ServiceBusSender> _serviceBusSendersCache = new();
    private readonly ISenderSettingsValidator _senderSettingsValidator;
    private readonly IServiceBusHelperLogger _logger;
    private SenderCallbacks _callbacks;
    private ServiceBusMessageSendData _messageToSend;
    private CancellationTokenSource _cancellationTokenSource;

    public MessageSender(ISenderSettingsValidator senderSettingsValidator, IServiceBusHelperLogger logger)
    {
        _senderSettingsValidator = senderSettingsValidator;
        _logger = logger;
    }

    public async Task Send(SenderCallbacks callbacks, ServiceBusMessageSendData messageToSend)
    {
        _callbacks = callbacks;
        _messageToSend = messageToSend;
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;
        try
        {
            _callbacks.OnStartSendingAction.Invoke();
            var validationResult = await _senderSettingsValidator.Validate(_messageToSend, token);
            if (validationResult.IsSomething())
            {
                _logger.LogError($"Sender '{messageToSend.ConfigName}' config validation failed: '{validationResult.Value().ErrorMsg}'");
                _callbacks.OnErrorHappenedWhileSending.Invoke(new MessageSendErrorInfo
                    { Message = "Send data validation failed. See log for details" });
                return;
            }
            
            await SendInternal(token);
            _callbacks.OnSendingFinishedSuccessfully.Invoke();
        }
        catch (Exception e) when (e is TaskCanceledException or OperationCanceledException)
        {
            _callbacks.OnSendingStopped.Invoke();
            _logger.LogWarning($"Sending message for configuration '{_messageToSend.ConfigName}' was stopped (operation/task was cancelled)");
        }
        catch (Exception e)
        {
            _logger.LogException(e);
            _callbacks.OnErrorHappenedWhileSending.Invoke(new MessageSendErrorInfo { Message = "Sending failed. See log for details" });
        }
    }

    public void Stop()
    {
        _callbacks.OnSendingStopping.Invoke();
        _cancellationTokenSource.Cancel();
    }

    private async Task SendInternal(CancellationToken cancellationToken)
    {
        var msgBody = _messageToSend.MsgBody;

        var client = GetClientForConnectionString(_messageToSend.ConnectionString);
        var sender = GetSenderForClient(client, _messageToSend.QueueOrTopicName, _messageToSend.ConnectionString);
        _logger.LogInfo($"Sending message to queue/topic '{_messageToSend.QueueOrTopicName}' with content\n: '{msgBody}'");

        var message = FillAllMessageFields(msgBody, _messageToSend.Fields, _messageToSend.ApplicationProperties);

        await sender.SendMessageAsync(message, cancellationToken);
        _callbacks.OnSendingFinishedSuccessfully.Invoke();
    }

    private static ServiceBusMessage FillAllMessageFields(string msgBody,
        SbMessageStandardFields messageFieldsToSend,
        IList<SBMessageApplicationProperty> messageApplicationPropertiesToSend)
    {
        if (msgBody == null)
        {
            msgBody = string.Empty;
        }

        ServiceBusMessage message = new ServiceBusMessage(msgBody);

        foreach (var appMessageProperty in messageApplicationPropertiesToSend)
        {
            if (!string.IsNullOrEmpty(appMessageProperty.PropertyName))
            {
                message.ApplicationProperties.Add(appMessageProperty.PropertyName, appMessageProperty.PropertyValue);
            }
        }

        SetMessageFieldIfEnabled(messageFieldsToSend.ContentType, ob => { message.ContentType = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.CorrelationId, ob => { message.CorrelationId = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.MessageId, ob => { message.MessageId = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.PartitionKey, ob => { message.PartitionKey = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.ReplyTo, ob => { message.ReplyTo = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.ReplyToSessionId, ob => { message.ReplyToSessionId = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.SessionId, ob => { message.SessionId = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.Subject, ob => { message.Subject = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.To, ob => { message.To = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.TransactionPartitionKey, ob => { message.TransactionPartitionKey = ob; });
        
        SetMessageFieldIfEnabled(messageFieldsToSend.TimeToLive, ob => { message.TimeToLive = ob; });
        SetMessageFieldIfEnabled(messageFieldsToSend.ScheduledEnqueueTime, ob =>
        {
            message.ScheduledEnqueueTime = ob;
        });

        return message;
    }

    private static void SetMessageFieldIfEnabled<TFieldValueType>(SbMessageField<TFieldValueType> field,
        Action<TFieldValueType> messagePropertySetter)
    {
        if (field.IsEnabled)
        {
            messagePropertySetter((TFieldValueType)field.ValueAsObject);
        }
    }

    private ServiceBusSender GetSenderForClient(ServiceBusClient client, string topicName, string connectionString)
    {
        var key = (connectionString, topicName);
        if (!_serviceBusSendersCache.ContainsKey(key))
        {
            _serviceBusSendersCache[key] = client.CreateSender(topicName);
        }

        return _serviceBusSendersCache[key];
    }

    private ServiceBusClient GetClientForConnectionString(string connectionString)
    {
        if (!_serviceBusClientsCache.ContainsKey(connectionString))
        {
            _serviceBusClientsCache[connectionString] = new ServiceBusClient(connectionString);
        }

        return _serviceBusClientsCache[connectionString];
    }
}

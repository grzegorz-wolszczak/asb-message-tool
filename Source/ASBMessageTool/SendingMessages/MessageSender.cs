using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ASBMessageTool.Application.Logging;
using ASBMessageTool.Model;
using Azure.Messaging.ServiceBus;
using Core.Maybe;

namespace ASBMessageTool.SendingMessages;

public record ServiceBusMessageSendData
{
    public string ConnectionString { get; init; }
    public string QueueOrTopicName { get; init; }
    public string MsgBody { get; init; }
    public SbMessageStandardFields Fields { get; init; }
    public IList<SBMessageApplicationProperty> ApplicationProperties { get; init; }
    public string ConfigName { get; init; }
}

public class MessageSender : IMessageSender
{
    private Dictionary<string /* connection string */, ServiceBusClient> _serviceBusClientsCache = new();
    private Dictionary<(string /* connection string */, string /*topicName*/), ServiceBusSender> _serviceBusSendersCache = new();
    private readonly ISenderSettingsValidator _senderSettingsValidator;
    private readonly IServiceBusHelperLogger _logger;

    public MessageSender(ISenderSettingsValidator senderSettingsValidator, IServiceBusHelperLogger logger)
    {
        _senderSettingsValidator = senderSettingsValidator;
        _logger = logger;
    }

    public async Task<Maybe<MessageSendErrorInfo>> Send(ServiceBusMessageSendData senderConfig)
    {
        try
        {
            var validationResult = await _senderSettingsValidator.Validate(senderConfig, CancellationToken.None);
            if (validationResult.IsSomething())
            {
                _logger.LogError($"Sender '{senderConfig.ConfigName}' config validation failed: '{validationResult.Value().ErrorMsg}'");
                return new MessageSendErrorInfo
                {
                    Message = "Send data validation failed. See log for details"
                }.ToMaybe();
            }

            return await SendInternal(senderConfig);
        }
        catch (Exception e)
        {
            _logger.LogException(e);
            var maybe = new MessageSendErrorInfo
            {
                Message = "Sending failed. See log for details"
            }.ToMaybe();
            return maybe;
        }
    }

    private async Task<Maybe<MessageSendErrorInfo>> SendInternal(ServiceBusMessageSendData msgSend)
    {
        var msgBody = msgSend.MsgBody;

        var client = GetClientForConnectionString(msgSend.ConnectionString);
        var sender = GetSenderForClient(client, msgSend.QueueOrTopicName, msgSend.ConnectionString);
        _logger.LogInfo($"Sending message to queue/topic '{msgSend.QueueOrTopicName}' with content\n: '{msgBody}'");

        var message = FillAllMessageFields(msgBody, msgSend.Fields, msgSend.ApplicationProperties);

        await sender.SendMessageAsync(message);
        return Maybe<MessageSendErrorInfo>.Nothing;
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

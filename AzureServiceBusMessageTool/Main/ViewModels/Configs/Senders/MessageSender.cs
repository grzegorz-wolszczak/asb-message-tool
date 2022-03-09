using System;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;
using Core.Maybe;
using Main.Application.Logging;
using Main.ViewModels.Configs.Senders.MessagePropertyWindow;

namespace Main.ViewModels.Configs.Senders
{
   public record MessageToSendData
   {
      public string ConnectionString { get; init; }
      public string TopicName { get; init; }
      public string MsgBody { get; init; }
      public SbMessageStandardFields Fields { get; init; }
      public IList<SBMessageApplicationProperty> ApplicationProperties { get; init; }
   }

   public class MessageSender : IMessageSender
   {
      private Dictionary<string /* connection string */, ServiceBusClient> _serviceBusClientsCache = new();
      private Dictionary<(string /* connection string */, string /*topicName*/), ServiceBusSender> _serviceBusSendersCache = new();
      private readonly IServiceBusHelperLogger _logger;

      public MessageSender(IServiceBusHelperLogger logger)
      {
         _logger = logger;
      }

      public Maybe<MessageSendErrorInfo> Send(MessageToSendData msgToSend)
      {
         try
         {
            var msgBody = msgToSend.MsgBody;

            var client = GetClientForConnectionString(msgToSend.ConnectionString);
            var sender = GetSenderForClient(client, msgToSend.TopicName, msgToSend.ConnectionString);
            _logger.LogInfo($"Sending message to topic '{msgToSend.TopicName}\nwith content :'{msgBody}'");

            var message = FillAllMessageFields(msgBody, msgToSend.Fields, msgToSend.ApplicationProperties);

            sender.SendMessageAsync(message).GetAwaiter().GetResult();
            return Maybe<MessageSendErrorInfo>.Nothing;
         }
         catch (Exception e)
         {
            _logger.LogException(e);
            return new MessageSendErrorInfo()
            {
               Message = "Sending failed. See log for details"
            }.ToMaybe();
         }
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

         SetMessageFieldIfEnabled<string>(messageFieldsToSend.ContentType, (ob) => { message.ContentType = ob;});
         SetMessageFieldIfEnabled<string>(messageFieldsToSend.CorrelationId, (ob) => { message.CorrelationId = ob;});
         SetMessageFieldIfEnabled<string>(messageFieldsToSend.MessageId, (ob) => { message.MessageId = ob;});
         SetMessageFieldIfEnabled<string>(messageFieldsToSend.PartitionKey, (ob) => { message.PartitionKey = ob;});
         SetMessageFieldIfEnabled<string>(messageFieldsToSend.ReplyTo, (ob) => { message.ReplyTo = ob;});
         SetMessageFieldIfEnabled<string>(messageFieldsToSend.ReplyToSessionId, (ob) => { message.ReplyToSessionId = ob;});
         SetMessageFieldIfEnabled<string>(messageFieldsToSend.SessionId, (ob) => { message.SessionId = ob;});
         SetMessageFieldIfEnabled<string>(messageFieldsToSend.Subject, (ob) => { message.Subject = ob;});
         SetMessageFieldIfEnabled<string>(messageFieldsToSend.To, (ob) => { message.To = ob;});
         SetMessageFieldIfEnabled<string>(messageFieldsToSend.TransactionPartitionKey, (ob) => { message.TransactionPartitionKey = ob;});

         return message;
      }

      private static void SetMessageFieldIfEnabled<FieldValueType>(SbMessageField<FieldValueType> field, Action<FieldValueType> messagePropertySetter)
      {
         if (field.IsEnabled)
         {
            messagePropertySetter((FieldValueType)field.ValueAsObject);
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
}

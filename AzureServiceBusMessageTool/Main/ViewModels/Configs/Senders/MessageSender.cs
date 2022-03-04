﻿using System;
using System.Collections.Generic;
using System.Text;
using Azure.Messaging.ServiceBus;
using Core.Maybe;
using Main.Application.Logging;

namespace Main.ViewModels.Configs.Senders;

public record MessageToSendData
{
   public string ConnectionString { get; init; }
   public string TopicName { get; init; }
   public string MsgBody { get; init; }
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
         if (msgBody == null)
         {
            _logger.LogInfo("Msg body is null, replacing with empty string");
            msgBody = string.Empty;
         }

         ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(msgBody));
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
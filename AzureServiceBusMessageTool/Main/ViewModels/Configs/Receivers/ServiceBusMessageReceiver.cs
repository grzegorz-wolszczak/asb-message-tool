﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Main.Application;
using Main.Application.Logging;
using Main.Utils;

namespace Main.ViewModels.Configs.Receivers
{
   public class ServiceBusMessageReceiver : IServiceBusMessageReceiver
   {
      private readonly IServiceBusHelperLogger _logger;
      private ServiceBusReceiver _receiver;
      private CancellationTokenSource _cancellationTokenSource;
      private ServiceBusReceiverSettings _config;
      private ReceiverCallbacks _callbacks;
      private ServiceBusClient _client;
      private StopReason _stopReason = StopReason.Intentional;
      private ReceivedMessageFormatter _msgFormatter;

      public ServiceBusMessageReceiver(IServiceBusHelperLogger logger)
      {
         _logger = logger;
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
            TryStart();
         }
         catch (Exception e)
         {
            StopReceiver();
            callbacks.OnReceiverFailure.Invoke(e);
            _logger.LogException($"Could not start receiving for receiver '{_config.ConfigName}' for messages because of error.", e);
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
         _receiver?.DisposeAsync();
      }

      private void TryStart()
      {
         if (_cancellationTokenSource != null)
         {
            throw new ServiceBusHelperException($"InternalError: Receiver '{_config.ConfigName}' already started");
         }

         _client = new ServiceBusClient(_config.ConnectionString);
         var serviceBusReceiverOptions = new ServiceBusReceiverOptions()
         {
            SubQueue = _config.IsDeadLetterQueue ? SubQueue.DeadLetter : SubQueue.None,
            ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete,
         };
         _receiver = _client.CreateReceiver(
            _config.TopicName,
            _config.SubscriptionName,
            serviceBusReceiverOptions);

         _cancellationTokenSource = new CancellationTokenSource();
         var token = _cancellationTokenSource.Token;

         Task.Factory.StartNew(async () =>
         {
            try
            {
               _logger.LogInfo($"Receiver '{_config.ConfigName}' started.");
               _callbacks.OnReceiverStarted.Invoke();
               while (!token.IsCancellationRequested)
               {
                  var message = await _receiver.ReceiveMessageAsync(StaticConfig.MessageReceiverReceiveTimeout, token);

                  if (message != null)
                  {
                     var receivedMessage = new ReceivedMessage()
                     {
                        Body = _msgFormatter.Format(message)
                     };
                     _callbacks.OnMessageReceive(receivedMessage);
                  }

                  await Task.Delay(_config.NextMessageReceiveDelayPeriod);
               }
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
}

using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using System.Collections.Specialized;

namespace Main.Utils
{
   public class ReceivedMessageFormatter
   {
      private int _maxFieldWith = 0;
      private OrderedDictionary _fieldValues = new();
      // application properties will be displayed with additional indent
      // make room for it for standard message properties
      private const int IndentOffsetForApplicationProperties = 4;

      public string Format(ServiceBusReceivedMessage msg)
      {
         _fieldValues.Clear();
         _maxFieldWith = 0;

         AppendFiledValues(msg);
         return CreateFormattedMsg();
      }

      private string CreateFormattedMsg()
      {
         var enumerator = _fieldValues.GetEnumerator();

         var sb = new StringBuilder();
         var formatString = $"{{0,-{_maxFieldWith + IndentOffsetForApplicationProperties}}}";
         while (enumerator.MoveNext())
         {
            sb.Append($"{String.Format(formatString, enumerator.Key)} : '{enumerator.Value}'\n");
         }

         return sb.ToString();
      }

      private void AppendFiledValues(ServiceBusReceivedMessage msg)
      {
         AppendNotEmptyPrimitiveProperties(msg);
         //AppendApplicationProperties(msg);
      }


      private void AppendApplicationProperties(ServiceBusReceivedMessage msg)
      {
         //throw new NotImplementedException();
      }

      private void AppendNotEmptyPrimitiveProperties(ServiceBusReceivedMessage msg)
      {
         AppendSimplePropertyIfNotNull(nameof(msg.ContentType), msg.ContentType);
         AppendSimplePropertyIfNotNull(nameof(msg.CorrelationId), msg.CorrelationId);
         AppendSimplePropertyIfNotNull(nameof(msg.DeadLetterErrorDescription), msg.DeadLetterErrorDescription);
         AppendSimplePropertyIfNotNull(nameof(msg.DeadLetterReason), msg.DeadLetterReason);
         AppendSimplePropertyIfNotNull(nameof(msg.DeadLetterSource), msg.DeadLetterSource);
         AppendSimplePropertyIfNotNull(nameof(msg.DeliveryCount), msg.DeliveryCount);
         AppendSimplePropertyIfNotNull(nameof(msg.EnqueuedSequenceNumber), msg.EnqueuedSequenceNumber);
         AppendSimplePropertyIfNotNull(nameof(msg.EnqueuedTime), msg.EnqueuedTime);
         AppendSimplePropertyIfNotNull(nameof(msg.ExpiresAt), msg.ExpiresAt);
         AppendSimplePropertyIfNotNull(nameof(msg.LockedUntil), msg.LockedUntil);
         AppendSimplePropertyIfNotNull(nameof(msg.LockToken), msg.LockToken);
         AppendSimplePropertyIfNotNull(nameof(msg.MessageId), msg.MessageId);
         AppendSimplePropertyIfNotNull(nameof(msg.PartitionKey), msg.PartitionKey);
         AppendSimplePropertyIfNotNull(nameof(msg.ReplyTo), msg.ReplyTo);
         AppendSimplePropertyIfNotNull(nameof(msg.ReplyToSessionId), msg.ReplyToSessionId);
         AppendSimplePropertyIfNotNull(nameof(msg.ScheduledEnqueueTime), msg.ScheduledEnqueueTime);
         AppendSimplePropertyIfNotNull(nameof(msg.SequenceNumber), msg.SequenceNumber);
         AppendSimplePropertyIfNotNull(nameof(msg.SessionId), msg.SessionId);
         AppendSimplePropertyIfNotNull(nameof(msg.State), msg.State);
         AppendSimplePropertyIfNotNull(nameof(msg.Subject), msg.Subject);
         AppendSimplePropertyIfNotNull(nameof(msg.TimeToLive), msg.TimeToLive);
         AppendSimplePropertyIfNotNull(nameof(msg.To), msg.To);
         AppendSimplePropertyIfNotNull(nameof(msg.TransactionPartitionKey), msg.TransactionPartitionKey);
         AppendSimplePropertyIfNotNull(nameof(msg.Body), msg.Body);
      }

      private void AppendSimplePropertyIfNotNull<T>(string fieldName, T fieldValue)
      {
         if (fieldValue == null)
         {
            return;
         }

         _maxFieldWith = Math.Max(_maxFieldWith, fieldName.Length + IndentOffsetForApplicationProperties);
         _fieldValues[fieldName] = $"{fieldValue.ToString()}";
      }
   }
}

using System;
using System.Collections;
using System.Text;
using Azure.Messaging.ServiceBus;
using System.Collections.Specialized;

namespace Main.Utils;

public class ReceivedMessageFormatter
{
    private int _maxFieldWidth = 0;
    private OrderedDictionary _fieldValues = new();
    // application properties will be displayed with additional indent
    // make room for it for standard message properties
    private readonly string applicationPropertiesIndent = new String(' ', 3);

    public string Format(ServiceBusReceivedMessage msg)
    {
        _fieldValues.Clear();
        _maxFieldWidth = 0;

        AppendFieldValues(msg);
        CalculateMaxFieldWidth(msg);

        var sb = new StringBuilder();
        var enumerator = _fieldValues.GetEnumerator();
        var formatString = $"{{0,-{_maxFieldWidth }}}";
        var appPropertyFormatString = $"{{0,-{_maxFieldWidth -applicationPropertiesIndent.Length }}}";
        while (enumerator.MoveNext())
        {
            sb.Append($"{String.Format(formatString, enumerator.Key)} : '{enumerator.Value}'\n");
        }

        var appProperties = msg.ApplicationProperties;

        if (appProperties != null && appProperties.Count>0)
        {
            sb.Append("\n####### Application properties:\n");

            foreach (var appProperty in appProperties)
            {
                sb.Append($"{applicationPropertiesIndent}{String.Format(appPropertyFormatString, appProperty.Key)} : '{appProperty.Value}'\n");
            }
            sb.Append('\n');
        }

        sb.Append($"{String.Format(formatString, "Body")} : '{msg.Body.ToString()}'\n");


        return sb.ToString();
    }

    private void CalculateMaxFieldWidth(ServiceBusReceivedMessage msg)
    {
        foreach (DictionaryEntry fieldValue in _fieldValues)
        {
            _maxFieldWidth = Math.Max(_maxFieldWidth, ((string)fieldValue.Key).Length + applicationPropertiesIndent.Length);
        }

        if (msg.ApplicationProperties == null)
        {
            return;
        }
        foreach (var msgApplicationProperty in msg.ApplicationProperties)
        {
            _maxFieldWidth = Math.Max(_maxFieldWidth , msgApplicationProperty.Key.Length + applicationPropertiesIndent.Length);
        }
    }


    private void AppendFieldValues(ServiceBusReceivedMessage msg)
    {
        AppendSimplePropertyIfNotNull(nameof(msg.DeadLetterErrorDescription), msg.DeadLetterErrorDescription);
        AppendSimplePropertyIfNotNull(nameof(msg.DeadLetterReason), msg.DeadLetterReason);
        AppendSimplePropertyIfNotNull(nameof(msg.DeadLetterSource), msg.DeadLetterSource);
        AppendSimplePropertyIfNotNull(nameof(msg.DeliveryCount), msg.DeliveryCount);
        AppendSimplePropertyIfNotNull(nameof(msg.EnqueuedSequenceNumber), msg.EnqueuedSequenceNumber);
        AppendSimplePropertyIfNotNull(nameof(msg.EnqueuedTime), msg.EnqueuedTime);
        AppendSimplePropertyIfNotNull(nameof(msg.ExpiresAt), msg.ExpiresAt);
        AppendSimplePropertyIfNotNull(nameof(msg.LockedUntil), msg.LockedUntil);
        AppendSimplePropertyIfNotNull(nameof(msg.LockToken), msg.LockToken);
        AppendSimplePropertyIfNotNull(nameof(msg.ScheduledEnqueueTime), msg.ScheduledEnqueueTime);
        AppendSimplePropertyIfNotNull(nameof(msg.SequenceNumber), msg.SequenceNumber);
        AppendSimplePropertyIfNotNull(nameof(msg.State), msg.State);
        AppendSimplePropertyIfNotNull(nameof(msg.TimeToLive), msg.TimeToLive);
        AppendSimplePropertyIfNotNull(nameof(msg.ContentType), msg.ContentType);
        AppendSimplePropertyIfNotNull(nameof(msg.CorrelationId), msg.CorrelationId);
        AppendSimplePropertyIfNotNull(nameof(msg.MessageId), msg.MessageId);
        AppendSimplePropertyIfNotNull(nameof(msg.PartitionKey), msg.PartitionKey);
        AppendSimplePropertyIfNotNull(nameof(msg.ReplyTo), msg.ReplyTo);
        AppendSimplePropertyIfNotNull(nameof(msg.ReplyToSessionId), msg.ReplyToSessionId);
        AppendSimplePropertyIfNotNull(nameof(msg.SessionId), msg.SessionId);
        AppendSimplePropertyIfNotNull(nameof(msg.Subject), msg.Subject);
        AppendSimplePropertyIfNotNull(nameof(msg.To), msg.To);
        AppendSimplePropertyIfNotNull(nameof(msg.TransactionPartitionKey), msg.TransactionPartitionKey);
        //AppendSimplePropertyIfNotNull(nameof(msg.Body), msg.Body);
    }


    private void AppendSimplePropertyIfNotNull<T>(string fieldName, T fieldValue)
    {
        if (fieldValue == null)
        {
            return;
        }


        _fieldValues[fieldName] = $"{fieldValue.ToString()}";
    }
}
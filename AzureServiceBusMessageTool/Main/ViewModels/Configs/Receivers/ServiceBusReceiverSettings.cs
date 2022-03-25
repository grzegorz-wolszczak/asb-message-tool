using System;
using System.Collections.Generic;
using System.Linq;
using Main.Models;
using Main.ViewModels.Configs.Senders;

namespace Main.ViewModels.Configs.Receivers;

public record ServiceBusReceiverSettings
{
    public string ConfigName { get; init; }
    public string ConnectionString { get; init; }
    public string TopicName { get; init; }
    public string SubscriptionName { get; init; }
    public bool IsDeadLetterQueue { get; init; }
    public TimeSpan MessageReceiveDelayPeriod { get; init; }

    public OnMessageReceiveEnumAction OnMessageReceiveEnumAction { get; init; }
    public IList<SBMessageApplicationProperty> AbandonMessageOverriddenApplicationProperties { get; init; }
    public IList<SBMessageApplicationProperty> DeadLetterMessageOverriddenApplicationProperties { get; init; }
    public SbDeadLetterMessageFields DeadLetterMessageFields { get; init; }
    public DeadLetterMessageFieldsOverrideEnumType DeadLetterMessageFieldsOverrideType { get; init; }

    public string ReceiverQueueName { get; init; }

    public ReceiverDataSourceType ReceiverDataSourceType { get; init; }
    public bool ShouldShowOnlyMessageBodyAsJson { get; init; }
    public bool ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter { get; init; }
}

public static class Extensions
{
    private static readonly Dictionary<string, object> EmptyPropertyDict = new();

    public static Dictionary<string, object> AsPropertyDictionary(this IList<SBMessageApplicationProperty> it)
    {
        if (it is { Count: <= 0 })
        {
            return EmptyPropertyDict;
        }

        return it.ToDictionary((e => e.PropertyName), v => (object)v.PropertyValue);

    }
}

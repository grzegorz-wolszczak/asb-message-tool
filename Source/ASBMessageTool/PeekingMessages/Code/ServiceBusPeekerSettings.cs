using ASBMessageTool.Model;

namespace ASBMessageTool.PeekingMessages.Code;

public record ServiceBusPeekerSettings
{
    public string ConfigName { get; init; }
    public string ConnectionString { get; init; }
    public string TopicName { get; init; }
    public string SubscriptionName { get; init; }
    public bool IsDeadLetterQueue { get; init; }
    public string ReceiverQueueName { get; init; }
    public ReceiverDataSourceType ReceiverDataSourceType { get; init; }
    public bool ShouldShowOnlyMessageBodyAsJson { get; init; }
    public bool ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter { get; init; }
}

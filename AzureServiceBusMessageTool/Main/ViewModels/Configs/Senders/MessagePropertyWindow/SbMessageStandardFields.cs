namespace Main.ViewModels.Configs.Senders.MessagePropertyWindow;

public interface ISbMessageField
{
    string FieldName { get; }
    bool IsEnabled { get; set; }
    object ValueAsObject { get; }
}

public class SbMessageField<T> : ISbMessageField
{
    public SbMessageField(string fieldName)
    {
        FieldName = fieldName;
    }

    public string FieldName { get; }
    public T Value { get; set; }
    public bool IsEnabled { get; set; } = false;
    public object ValueAsObject => Value;
    public T ValueIfEnabledOrNull => IsEnabled ? Value : default;

}

public class SbMessageStandardFields
{
    public SbMessageField<string> ContentType { get; } = new("ContentType");
    public SbMessageField<string> CorrelationId { get; } = new("CorrelationId");
    public SbMessageField<string> MessageId { get; } = new("MessageId");
    public SbMessageField<string> PartitionKey { get; } = new("PartitionKey");
    public SbMessageField<string> ReplyTo { get; } = new("ReplyTo");
    public SbMessageField<string> ReplyToSessionId { get; } = new("ReplyToSessionId");
    public SbMessageField<string> SessionId { get; } = new("SessionId");
    public SbMessageField<string> Subject { get; } = new("Subject");
    public SbMessageField<string> To { get; } = new("To");
    public SbMessageField<string> TransactionPartitionKey { get; } = new("TransactionPartitionKey");

    //public TimeSpan? TimeToLive { get; set; } // todo: no dedicated WPF control to store TimeSpan, must find something
    //public DateTimeOffset? ScheduledEnqueueTimeUtc { get; set; } // todo: no dedicated WPF control to store DateTimeOffset, must find something
}

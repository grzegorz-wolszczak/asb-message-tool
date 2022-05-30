using System;

namespace ASBMessageTool.Model;

public class SbMessageField<T>
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
    public static readonly TimeSpan DefaultTimeToLive = TimeSpan.FromDays(14);
    public static readonly TimeSpan MaxTimeToLive = TimeSpan.FromDays(14);
    
    public static readonly DateTime MinScheduleEnqueueTime = DateTime.MinValue;
    public static readonly DateTime DefaultScheduleEnqueueTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    
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
    
    public SbMessageField<TimeSpan> TimeToLive { get; } = new("TimeToLive")
    {
        Value = DefaultTimeToLive
    };
    
    public SbMessageField<DateTime> ScheduledEnqueueTime { get; } = new("ScheduledEnqueueTime")
    {
        Value = DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc)
    };
    
}

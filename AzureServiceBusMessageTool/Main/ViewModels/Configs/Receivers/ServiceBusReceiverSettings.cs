using System;

namespace Main.ViewModels.Configs.Receivers;

public class ServiceBusReceiverSettings
{
    public string ConfigName { get; init; }
    public string ConnectionString { get; init; }
    public string TopicName { get; init; }
    public string SubscriptionName { get; init; }
    public bool IsDeadLetterQueue { get; init; }
    public TimeSpan MessageReceiveDelayPeriod { get; init; }
}

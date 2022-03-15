using Azure.Messaging.ServiceBus;

namespace Main.ViewModels.Configs.Receivers;

public class ReceivedMessage
{
    public ServiceBusReceivedMessage OriginalMessage { get; init; }
}

using Azure.Messaging.ServiceBus;

namespace ASBMessageTool.ReceivingMessages;

public class ReceivedMessage
{
    public ServiceBusReceivedMessage OriginalMessage { get; init; }
}

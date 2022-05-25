using Azure.Messaging.ServiceBus;

namespace ASBMessageTool.ReceivingMessages.Code;

public class ReceivedMessage
{
    public ServiceBusReceivedMessage OriginalMessage { get; init; }
}

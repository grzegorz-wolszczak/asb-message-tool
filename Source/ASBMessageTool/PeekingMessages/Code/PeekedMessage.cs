using Azure.Messaging.ServiceBus;

namespace ASBMessageTool.PeekingMessages.Code;

public class PeekedMessage
{
    public ServiceBusReceivedMessage OriginalMessage { get; init; }
}

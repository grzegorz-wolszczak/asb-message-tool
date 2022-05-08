using System;

namespace ASBMessageTool.ReceivingMessages;

public class ReceiverCallbacks
{
    public Action<Exception> OnReceiverFailure { get; init; }
    public Action<ReceivedMessage> OnMessageReceive { get; init; }
    public Action OnReceiverStop { get; init; }
    public Action OnReceiverStarted { get; init; }

    public Action OnReceiverInitializing { get; init; }
}

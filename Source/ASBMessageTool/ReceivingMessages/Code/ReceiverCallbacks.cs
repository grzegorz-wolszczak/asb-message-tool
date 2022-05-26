using System;

namespace ASBMessageTool.ReceivingMessages.Code;

public class ReceiverCallbacks
{
    public Action<Exception> OnReceiverFailure { get; init; }
    public Action<ReceivedMessage> OnMessageReceive { get; init; }
    public Action OnReceiverStop { get; init; }
    public Action OnReceiverStarted { get; init; }

    public Action OnReceiverInitializing { get; init; }
    
    public Action<string> OnOutputFromReceiverReceived { get; init; }
}

using System;

namespace Main.ViewModels.Configs.Receivers;

public class ReceiverCallbacks
{
    public Action<Exception> OnReceiverFailure { get; init; }
    public Action<ReceivedMessage> OnMessageReceive { get; init; }
    public Action OnReceiverStop { get; init; }
    public Action OnReceiverStarted { get; init; }
}
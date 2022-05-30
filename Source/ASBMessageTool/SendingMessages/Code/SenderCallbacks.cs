using System;

namespace ASBMessageTool.SendingMessages.Code;

public record SenderCallbacks
{
    public Action OnSendingFinishedSuccessfully { get; init; }
    
    public Action OnSendingStopped { get; init; }
    public Action OnSendingStopping { get; init; }

    public Action<MessageSendErrorInfo> OnErrorHappenedWhileSending { get; init; }
    public Action OnStartSendingAction { get; init; }
}

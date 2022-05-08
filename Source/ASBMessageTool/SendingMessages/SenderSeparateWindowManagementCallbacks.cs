using System;

namespace ASBMessageTool.SendingMessages;

public class SenderSeparateWindowManagementCallbacks
{
    public Action OnAttachAction { get; init; }
    public Action OnDetachAction { get; init; }
}

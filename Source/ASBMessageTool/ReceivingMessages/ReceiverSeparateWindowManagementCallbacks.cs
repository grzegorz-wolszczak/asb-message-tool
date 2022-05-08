using System;

namespace ASBMessageTool.ReceivingMessages;

public class ReceiverSeparateWindowManagementCallbacks
{
    public Action OnAttachAction { get; init; }
    public Action OnDetachAction { get; init; }
}

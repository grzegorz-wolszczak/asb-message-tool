using System;

namespace ASBMessageTool.Application;

public class SeparateWindowManagementCallbacks
{
    public Action OnAttachAction { get; init; }
    public Action OnDetachAction { get; init; }
}

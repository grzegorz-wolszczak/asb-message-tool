using System;

namespace ASBMessageTool.Application;

public interface IInGuiThreadActionCaller
{
    public void Call(Action action);
}

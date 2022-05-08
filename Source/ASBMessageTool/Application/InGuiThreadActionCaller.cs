using System;

namespace ASBMessageTool.Application;

public class InGuiThreadActionCaller : IInGuiThreadActionCaller
{
    public void Call(Action action)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(action);
    }
}

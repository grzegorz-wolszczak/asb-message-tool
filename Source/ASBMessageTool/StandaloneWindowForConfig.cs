using System;

namespace ASBMessageTool;

public class StandaloneWindowForConfig<TWindowType> where TWindowType: System.Windows.Window
{
    private readonly TWindowType _window;
    private readonly Action _beforeHideAction;
    private readonly Action _beforeCloseAction;

    public StandaloneWindowForConfig(object config,
        TWindowType senderConfigWindow,
        Action beforeHideAction,
        Action beforeCloseAction)
    {
        _window = senderConfigWindow;
        _beforeHideAction = beforeHideAction;
        _beforeCloseAction = beforeCloseAction;
        _window.DataContext = config;
    }

    public void ShowConfig()
    {
        _window.Show();
    }

    public void Hide()
    {
        _beforeHideAction.Invoke();
        _window.Close();
    }

    public void Close()
    {
        //_window.ShouldHideOnClose = false;
        _beforeCloseAction.Invoke();
        _window.Close();
    }
}

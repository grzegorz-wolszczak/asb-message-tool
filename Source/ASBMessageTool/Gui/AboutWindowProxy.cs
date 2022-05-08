using ASBMessageTool.Application;
using ASBMessageTool.Gui.Views;

namespace ASBMessageTool.Gui;

public class AboutWindowProxy : IAboutWindowProxy
{
    private readonly AboutWindow _aboutWindow;

    public AboutWindowProxy(AboutWindow aboutWindow)
    {
        _aboutWindow = aboutWindow;
    }

    public void ShowWindow()
    {
        _aboutWindow.Show();
    }
}

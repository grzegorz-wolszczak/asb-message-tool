using System.Windows;
using ASBMessageTool.Gui;
using ASBMessageTool.SendingMessages.Gui;

namespace ASBMessageTool.SendingMessages.Code;

public class SenderConfigStandaloneWindowViewer
{
    private StandaloneWindowForConfig<SenderConfigWindow> _windowForConfig;

    public void ShowAsDetachedWindow()
    {
        _windowForConfig?.ShowConfig();
    }

    public void HideWindow()
    {
        _windowForConfig?.Hide();
    }

    public void DeleteWindow()
    {
        _windowForConfig?.Close();
    }

    public void SetDataContext(SenderConfigViewModel viewModel)
    {
        var senderConfigWindow = new SenderConfigWindow(() => { viewModel.IsContentDetached = false; });

        // make sure that when config name changes, window title also changes
        senderConfigWindow.SetBinding(Window.TitleProperty, new System.Windows.Data.Binding()
        {
            Path = new PropertyPath(nameof(viewModel.ModelItem.ConfigName)),
            Source = viewModel.ModelItem,
        });

        
        _windowForConfig = new StandaloneWindowForConfig<SenderConfigWindow>(
            viewModel,
            senderConfigWindow,
            () => { senderConfigWindow.ShouldHideOnClose = true; },
            () => { senderConfigWindow.ShouldHideOnClose = false; });
    }
}

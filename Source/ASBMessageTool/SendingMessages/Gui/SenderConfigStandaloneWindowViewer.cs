namespace ASBMessageTool.SendingMessages.Gui;

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
        var senderConfigWindow = new SenderConfigWindow(onCloseAction: () => { viewModel.IsContentDetached = false; });

        _windowForConfig = new StandaloneWindowForConfig<SenderConfigWindow>(
            viewModel,
            senderConfigWindow,
            beforeHideAction: () => { senderConfigWindow.ShouldHideOnClose = true; },
            beforeCloseAction: () => { senderConfigWindow.ShouldHideOnClose = false; });
    }
}

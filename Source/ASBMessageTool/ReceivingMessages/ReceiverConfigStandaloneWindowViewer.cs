using ASBMessageTool.ReceivingMessages.Gui;

namespace ASBMessageTool.ReceivingMessages;

public class ReceiverConfigStandaloneWindowViewer
{
    private StandaloneWindowForConfig<ReceiverConfigWindow> _windowForConfig;


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

    public void SetDataContext(ReceiverConfigViewModel viewModel)
    {
        var receiverConfigWindow =
            new ReceiverConfigWindow(onCloseAction: () => { viewModel.IsContentDetached = false; });

        _windowForConfig = new StandaloneWindowForConfig<ReceiverConfigWindow>(
            viewModel,
            receiverConfigWindow,
            beforeHideAction: () => { receiverConfigWindow.ShouldHideOnClose = true; },
            beforeCloseAction: () => { receiverConfigWindow.ShouldHideOnClose = false; });
    }
}

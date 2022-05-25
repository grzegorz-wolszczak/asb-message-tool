using System.Windows;
using ASBMessageTool.Gui;
using ASBMessageTool.ReceivingMessages.Gui;

namespace ASBMessageTool.ReceivingMessages.Code;

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

        // make sure that when config name changes, window title also changes
        receiverConfigWindow.SetBinding(Window.TitleProperty, new System.Windows.Data.Binding()
        {
            Path = new PropertyPath(nameof(viewModel.ModelItem.ConfigName)),
            Source = viewModel.ModelItem,
        });


        _windowForConfig = new StandaloneWindowForConfig<ReceiverConfigWindow>(
            viewModel,
            receiverConfigWindow,
            beforeHideAction: () => { receiverConfigWindow.ShouldHideOnClose = true; },
            beforeCloseAction: () => { receiverConfigWindow.ShouldHideOnClose = false; }
        );
    }
}

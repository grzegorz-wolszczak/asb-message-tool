using ASBMessageTool.ReceivingMessages.Gui;

namespace ASBMessageTool.ReceivingMessages;

public class DeadLetterMessagePropertiesWindowProxy : IDeadLetterMessagePropertiesWindowProxy
{
    private DeadLetterMessagePropertiesWindow _window;

    public DeadLetterMessagePropertiesWindowProxy()
    {
        _window = new DeadLetterMessagePropertiesWindow();
    }

    public void ShowDialog(DeadLetterMessagePropertiesViewModel viewModel)
    {
        _window.ShowDialogForDataContext(viewModel);
    }
}

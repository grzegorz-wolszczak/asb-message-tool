namespace ASBMessageTool.ReceivingMessages;

public interface IDeadLetterMessagePropertiesWindowProxy
{
    void ShowDialog(DeadLetterMessagePropertiesViewModel viewModel);
}

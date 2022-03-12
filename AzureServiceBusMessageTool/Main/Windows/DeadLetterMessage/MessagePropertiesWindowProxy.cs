using Main.ViewModels.Configs;

namespace Main.Windows.DeadLetterMessage;

public interface IDeadLetterMessagePropertiesWindowProxy
{
    void ShowDialog(DeadLetterMessagePropertiesViewModel viewModel);
}

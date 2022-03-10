using Main.ViewModels.Configs.Senders;
using Main.Windows.Configs;

namespace Main.ConfigsGuiMetadata;

public class SenderConfigElementGuiRepresentation
{
    private SenderConfigWindow _window;

    public void ShowCorrespondingElementWindow(SenderConfigViewModelWrapper currentSelectedItem)
    {
        if (_window == null)
        {
            _window = CreateWindowForElement(currentSelectedItem);
        }

        _window.Show();
    }

    private SenderConfigWindow CreateWindowForElement(SenderConfigViewModelWrapper currentSelectedItem)
    {
        var window = new SenderConfigWindow(currentSelectedItem);
        return window;
    }

    public void CloseWindowOnElementDelete()
    {
        // this can be null if we are deleting config and never actually showed a window
        _window?.Close();
    }
}
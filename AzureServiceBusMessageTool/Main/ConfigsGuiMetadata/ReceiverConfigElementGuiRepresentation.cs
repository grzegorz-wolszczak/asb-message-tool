using Main.ViewModels.Configs.Receivers;
using Main.Windows.Configs;

namespace Main.ConfigsGuiMetadata;

public class ReceiverConfigElementGuiRepresentation
{

    private ReceiverConfigWindow _window;


    public void ShowCorrespondingElementWindow(ReceiverConfigViewModelWrapper currentSelectedItem)
    {
        if (_window == null)
        {
            _window = CreateWindowForElement(currentSelectedItem);
        }

        _window.Show();
    }

    private ReceiverConfigWindow CreateWindowForElement(ReceiverConfigViewModelWrapper currentSelectedItem)
    {
        var window = new ReceiverConfigWindow(currentSelectedItem);
        return window;
    }

    public void CloseWindowOnElementDelete()
    {
        // this can be null if we are deleting config and never actually showed a window
        _window?.Close();
    }
}
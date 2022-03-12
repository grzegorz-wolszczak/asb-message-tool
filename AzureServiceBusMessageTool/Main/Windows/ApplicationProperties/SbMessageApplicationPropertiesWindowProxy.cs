using Main.ViewModels;

namespace Main.Windows.ApplicationProperties;

public class MessageApplicationPropertiesWindowProxy : IMessageApplicationPropertiesWindowProxy
{
    private SbMessageApplicationPropertiesWindow _window;

    public MessageApplicationPropertiesWindowProxy()
    {
        _window = new SbMessageApplicationPropertiesWindow();
    }

    public void ShowDialog(SbMessageApplicationPropertiesViewModel messageApplicationProperties)
    {
        _window.ShowDialogForDataContext(messageApplicationProperties);
    }
}

public interface IMessageApplicationPropertiesWindowProxy
{
    void ShowDialog(SbMessageApplicationPropertiesViewModel messageApplicationProperties);
}

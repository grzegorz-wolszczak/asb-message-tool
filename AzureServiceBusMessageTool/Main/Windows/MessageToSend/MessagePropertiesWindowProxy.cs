using Main.ViewModels.Configs.Senders;

namespace Main.Windows.MessageToSend;

public class SenderMessagePropertiesWindowProxy : ISenderMessagePropertiesWindowProxy
{
    private SenderMessagePropertiesWindow _window;

    public SenderMessagePropertiesWindowProxy()
    {
        _window = new SenderMessagePropertiesWindow();
    }

    public void ShowDialog(SbMessageFieldsViewModel messageApplicationProperties)
    {
        _window.ShowDialogForDataContext(messageApplicationProperties);
    }
}

public interface ISenderMessagePropertiesWindowProxy
{
    void ShowDialog(SbMessageFieldsViewModel messageApplicationProperties);
}

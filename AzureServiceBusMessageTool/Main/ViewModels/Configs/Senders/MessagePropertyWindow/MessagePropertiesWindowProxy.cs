using Main.Windows;

namespace Main.ViewModels.Configs.Senders.MessagePropertyWindow;

public class MessagePropertiesWindowProxy : IMessagePropertiesWindowProxy
{
    private MessagePropertiesWindow _window;

    public MessagePropertiesWindowProxy()
    {
        _window = new MessagePropertiesWindow();
    }

    public void ShowDialog(SbMessageFieldsViewModel messageApplicationProperties)
    {
        _window.ShowDialogForDataContext(messageApplicationProperties);
    }
}

public interface IMessagePropertiesWindowProxy
{
    void ShowDialog(SbMessageFieldsViewModel messageApplicationProperties);
}

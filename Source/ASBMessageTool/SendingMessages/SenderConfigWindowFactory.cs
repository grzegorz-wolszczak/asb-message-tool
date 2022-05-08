using ASBMessageTool.SendingMessages.Gui;

namespace ASBMessageTool.SendingMessages;

public class SenderConfigWindowFactory : ISenderConfigWindowFactory
{
    public SenderConfigStandaloneWindowViewer CreateWindowForConfig()
    {
        return new SenderConfigStandaloneWindowViewer();
    }
}

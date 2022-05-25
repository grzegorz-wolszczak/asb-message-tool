namespace ASBMessageTool.SendingMessages.Code;

public class SenderConfigWindowFactory : ISenderConfigWindowFactory
{
    public SenderConfigStandaloneWindowViewer CreateWindowForConfig()
    {
        return new SenderConfigStandaloneWindowViewer();
    }
}

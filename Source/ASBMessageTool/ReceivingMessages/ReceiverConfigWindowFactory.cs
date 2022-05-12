namespace ASBMessageTool.ReceivingMessages;

public class ReceiverConfigWindowFactory 
{
    public ReceiverConfigStandaloneWindowViewer CreateWindowForConfig()
    {
        return new ReceiverConfigStandaloneWindowViewer();
    }
}

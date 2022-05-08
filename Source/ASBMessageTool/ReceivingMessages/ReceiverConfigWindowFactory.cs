namespace ASBMessageTool.ReceivingMessages;

public class ReceiverConfigWindowFactory // todo: implement that + make use interface wherever possible
{
    public ReceiverConfigStandaloneWindowViewer CreateWindowForConfig()
    {
        return new ReceiverConfigStandaloneWindowViewer();
    }
}

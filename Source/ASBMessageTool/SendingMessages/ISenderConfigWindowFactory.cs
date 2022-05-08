using ASBMessageTool.SendingMessages.Gui;

namespace ASBMessageTool.SendingMessages;

public interface ISenderConfigWindowFactory
{
    SenderConfigStandaloneWindowViewer CreateWindowForConfig();
}

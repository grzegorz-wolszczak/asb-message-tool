using ASBMessageTool.Application.Logging;

namespace ASBMessageTool.SendingMessages;

public class MessageSenderFactory
{
    private readonly IServiceBusHelperLogger _logger;

    public MessageSenderFactory(IServiceBusHelperLogger logger)
    {
        _logger = logger;
    }

    public IMessageSender Create()
    {
        var validator = new SenderSettingsValidator();
        return new MessageSender(validator,_logger);
    }
}

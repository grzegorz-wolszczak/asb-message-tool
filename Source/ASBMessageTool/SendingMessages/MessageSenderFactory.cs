using ASBMessageTool.Application.Logging;

namespace ASBMessageTool.SendingMessages;

public class MessageSenderFactory
{
    private readonly IServiceBusHelperLogger _logger;
    private readonly ISenderSettingsValidator _validator;

    public MessageSenderFactory(IServiceBusHelperLogger logger, ISenderSettingsValidator senderSettingsValidator)
    {
        _logger = logger;
        _validator = senderSettingsValidator;
    }

    public IMessageSender Create()
    {
        return new MessageSender(_validator,_logger);
    }
}

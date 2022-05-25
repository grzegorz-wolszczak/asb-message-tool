using ASBMessageTool.Application.Logging;

namespace ASBMessageTool.ReceivingMessages.Code;

public class ServiceBusMessageReceiverFactory
{
    private readonly IServiceBusHelperLogger _logger;
    private readonly IReceiverSettingsValidator _receiverSettingsValidator;

    public ServiceBusMessageReceiverFactory(IServiceBusHelperLogger logger, IReceiverSettingsValidator receiverSettingsValidator)
    {
        _logger = logger;
        _receiverSettingsValidator = receiverSettingsValidator;
    }

    public IServiceBusMessageReceiver Create()
    {
        return new ServiceBusMessageReceiver(_logger, _receiverSettingsValidator);
    }
}

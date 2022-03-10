using Main.Application.Logging;

namespace Main.ViewModels.Configs.Receivers;

public class ServiceBusMessageReceiverFactory
{
    private readonly IServiceBusHelperLogger _logger;

    public ServiceBusMessageReceiverFactory(IServiceBusHelperLogger logger)
    {
        _logger = logger;
    }

    public IServiceBusMessageReceiver Create()
    {
        return new ServiceBusMessageReceiver(_logger);
    }
}
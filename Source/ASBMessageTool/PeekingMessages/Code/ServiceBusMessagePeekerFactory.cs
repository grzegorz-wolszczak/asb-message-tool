using ASBMessageTool.Application.Logging;

namespace ASBMessageTool.PeekingMessages.Code;

public class ServiceBusMessagePeekerFactory
{
    private readonly IServiceBusHelperLogger _logger;
    private readonly IPeekerSettingsValidator _peekerSettingsValidator;

    public ServiceBusMessagePeekerFactory(IServiceBusHelperLogger logger, IPeekerSettingsValidator peekerSettingsValidator)
    {
        _logger = logger;
        _peekerSettingsValidator = peekerSettingsValidator;
    }

    public IServiceBusMessagePeeker Create()
    {
        return new ServiceBusMessageMessagePeeker(_peekerSettingsValidator, _logger);
    }
}

public interface IServiceBusMessagePeeker
{
    void Start(ServiceBusPeekerSettings config, PeekerCallbacks callbacks);
    void Stop();
}

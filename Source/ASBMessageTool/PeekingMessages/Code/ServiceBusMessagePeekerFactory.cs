using System.Threading.Tasks;
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
        return new ServiceBusMessagePeeker(_peekerSettingsValidator, _logger);
    }
}

public interface IServiceBusMessagePeeker
{
    Task Start(ServiceBusPeekerSettings config, PeekerCallbacks callbacks);
    void Stop();
}

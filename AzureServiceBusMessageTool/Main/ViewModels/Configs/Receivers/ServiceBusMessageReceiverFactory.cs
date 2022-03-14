using System;
using Main.Application.Logging;
using Main.Validations;

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
        var receiverSettingsValidator = new ReceiverSettingsValidator(_logger);
        return new ServiceBusMessageReceiver(_logger, receiverSettingsValidator);
    }
}

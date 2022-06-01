using ASBMessageTool.Application;
using ASBMessageTool.Application.Logging;
using ASBMessageTool.ReceivingMessages.Code;

namespace ASBMessageTool.SendingMessages.Code;

public class SenderConfigViewModelFactory
{
    private readonly IServiceBusHelperLogger _logger;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly MessageSenderFactory _messageSenderFactory;
    private readonly SenderMessagePropertiesWindowProxyFactory _senderMessagePropertiesWindowProxyFactory;
    private readonly ISenderSettingsValidator _senderSettingsValidator;
    private readonly IOperationSystemServices _operationSystemServices;
 

    public SenderConfigViewModelFactory(IServiceBusHelperLogger logger,
        IInGuiThreadActionCaller inGuiThreadActionCaller,
        MessageSenderFactory messageSenderFactory,
        SenderMessagePropertiesWindowProxyFactory senderMessagePropertiesWindowProxyFactory,
        ISenderSettingsValidator senderSettingsValidator,
        IOperationSystemServices operationSystemServices)
    {
        _logger = logger;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _messageSenderFactory = messageSenderFactory;
        _senderMessagePropertiesWindowProxyFactory = senderMessagePropertiesWindowProxyFactory;
        _senderSettingsValidator = senderSettingsValidator;
        _operationSystemServices = operationSystemServices;
    }

    public SenderConfigViewModel Create(
        SenderConfigModel configModel,
        SenderConfigStandaloneWindowViewer windowForSenderConfig)
    {
        var callbacks = new SeparateWindowManagementCallbacks()
        {
            OnAttachAction = windowForSenderConfig.HideWindow,
            OnDetachAction = windowForSenderConfig.ShowAsDetachedWindow
        };
        
        var viewModel = new SenderConfigViewModel(
            configModel,
            callbacks,
            _senderMessagePropertiesWindowProxyFactory.Create(),
            _messageSenderFactory.Create(),
            _inGuiThreadActionCaller,
            _logger,
            _senderSettingsValidator,
            _operationSystemServices);
        return viewModel;
    }
}

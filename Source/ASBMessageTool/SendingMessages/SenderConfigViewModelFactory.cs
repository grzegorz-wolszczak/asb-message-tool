using ASBMessageTool.Application;
using ASBMessageTool.Application.Logging;
using ASBMessageTool.SendingMessages.Gui;

namespace ASBMessageTool.SendingMessages;

public class SenderConfigViewModelFactory
{
    private readonly IServiceBusHelperLogger _logger;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly MessageSenderFactory _messageSenderFactory;
    private readonly SenderMessagePropertiesWindowProxyFactory _senderMessagePropertiesWindowProxyFactory;
    private readonly ISenderSettingsValidator _senderSettingsValidator;

    public SenderConfigViewModelFactory(IServiceBusHelperLogger logger,
        IInGuiThreadActionCaller inGuiThreadActionCaller,
        MessageSenderFactory messageSenderFactory,
        SenderMessagePropertiesWindowProxyFactory senderMessagePropertiesWindowProxyFactory,
        ISenderSettingsValidator senderSettingsValidator)
    {
        _logger = logger;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _messageSenderFactory = messageSenderFactory;
        _senderMessagePropertiesWindowProxyFactory = senderMessagePropertiesWindowProxyFactory;
        _senderSettingsValidator = senderSettingsValidator;
    }

    public SenderConfigViewModel Create(
        SenderConfigModel senderConfigModel,
        SenderConfigStandaloneWindowViewer windowForSenderConfig)
    {
        var callbacks = new SeparateWindowManagementCallbacks()
        {
            OnAttachAction = windowForSenderConfig.HideWindow,
            OnDetachAction = windowForSenderConfig.ShowAsDetachedWindow
        };

        var viewModel = new SenderConfigViewModel(
            senderConfigModel,
            callbacks,
            _senderMessagePropertiesWindowProxyFactory.Create(),
            _messageSenderFactory.Create(),
            _inGuiThreadActionCaller,
            _logger,
            _senderSettingsValidator);
        return viewModel;
    }
}

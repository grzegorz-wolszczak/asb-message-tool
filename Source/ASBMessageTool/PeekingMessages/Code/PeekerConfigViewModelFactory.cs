using ASBMessageTool.Application;
using ASBMessageTool.ReceivingMessages.Code;

namespace ASBMessageTool.PeekingMessages.Code;

public class PeekerConfigViewModelFactory
{
    private readonly ReceivedMessageFormatter _receivedMessageFormatter;
    private readonly ServiceBusMessagePeekerFactory _serviceBusMessagePeekerFactory;
    private readonly IPeekerSettingsValidator _peekerSettingsValidator;
    private readonly InGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly OperationSystemServices _operationSystemServices;

    public PeekerConfigViewModelFactory(ReceivedMessageFormatter receivedMessageFormatter,
        ServiceBusMessagePeekerFactory serviceBusMessagePeekerFactory,
        IPeekerSettingsValidator peekerSettingsValidator,
        InGuiThreadActionCaller inGuiThreadActionCaller,
        OperationSystemServices operationSystemServices)
    {
        _receivedMessageFormatter = receivedMessageFormatter;
        _serviceBusMessagePeekerFactory = serviceBusMessagePeekerFactory;
        _peekerSettingsValidator = peekerSettingsValidator;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _operationSystemServices = operationSystemServices;
    }


    public PeekerConfigViewModel Create(PeekerConfigModel configModel, PeekerConfigStandaloneWindowViewer windowForSenderConfig)
    {
        var callbacks = new SeparateWindowManagementCallbacks()
        {
            OnAttachAction = windowForSenderConfig.HideWindow,
            OnDetachAction = windowForSenderConfig.ShowAsDetachedWindow
        };

        var viewModel = new PeekerConfigViewModel( 
            configModel,
            callbacks,
            _serviceBusMessagePeekerFactory.Create(),
            _receivedMessageFormatter,
            _peekerSettingsValidator,
            _inGuiThreadActionCaller,
            _operationSystemServices
        );
        return viewModel;
    }
}

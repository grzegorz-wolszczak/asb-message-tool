﻿using ASBMessageTool.Application;

namespace ASBMessageTool.ReceivingMessages;

public class ReceiverConfigViewModelFactory
{
    private readonly ReceivedMessageFormatter _receivedMessageFormatter;
    private readonly DeadLetterMessagePropertiesWindowProxyFactory _deadLetterMessagePropertiesWindowProxyFactory;
    private readonly MessagePropertiesWindowProxyFactory _messagePropertiesWindowProxyFactory;
    private readonly ServiceBusMessageReceiverFactory _serviceBusMessageReceiverFactory;
    private readonly IReceiverSettingsValidator _receiverSettingsValidator;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;

    public ReceiverConfigViewModelFactory(
        ReceivedMessageFormatter receivedMessageFormatter,
        DeadLetterMessagePropertiesWindowProxyFactory deadLetterMessagePropertiesWindowProxyFactory,
        MessagePropertiesWindowProxyFactory messagePropertiesWindowProxyFactory,
        ServiceBusMessageReceiverFactory serviceBusMessageReceiverFactory,
        IReceiverSettingsValidator receiverSettingsValidator,
        IInGuiThreadActionCaller inGuiThreadActionCaller)
    {
        _receivedMessageFormatter = receivedMessageFormatter;
        _deadLetterMessagePropertiesWindowProxyFactory = deadLetterMessagePropertiesWindowProxyFactory;
        _messagePropertiesWindowProxyFactory = messagePropertiesWindowProxyFactory;
        _serviceBusMessageReceiverFactory = serviceBusMessageReceiverFactory;
        _receiverSettingsValidator = receiverSettingsValidator;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
    }

    public ReceiverConfigViewModel Create(ReceiverConfigModel item,
        SeparateWindowManagementCallbacks callbacks)
    {
        return new ReceiverConfigViewModel(
            item,
            callbacks, 
            _serviceBusMessageReceiverFactory.Create(), 
            _messagePropertiesWindowProxyFactory.Create(),
            _deadLetterMessagePropertiesWindowProxyFactory.Create(), 
            _receivedMessageFormatter,
            _receiverSettingsValidator,
            _inGuiThreadActionCaller);
    }
}
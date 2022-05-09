using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ASBMessageTool.Application.Logging;

namespace ASBMessageTool.ReceivingMessages;

public class ReceiversConfigsViewModel : INotifyPropertyChanged
{
    private readonly ObservableCollection<ReceiverConfigViewModel> _receiverConfigViewModels;
    private readonly ReceiverConfigWindowFactory _receiverConfigWindowFactory;
    private readonly IServiceBusHelperLogger _logger;
    private ReceiverConfigViewModel _currentSelectedItem;
    private ReceivedMessageFormatter _receivedMessageFormatter;
    private ServiceBusMessageReceiverFactory _serviceBusMessageReceiverFactory;
    private readonly MessagePropertiesWindowProxyFactory _messagePropertiesWindowProxyFactory;
    private DeadLetterMessagePropertiesWindowProxyFactory _deadLetterMessagePropertiesWindowProxyFactory;
    private readonly Dictionary<ReceiverConfigViewModel, ReceiverConfigStandaloneWindowViewer> _windowsForItems = new();
    private readonly ReceiversConfigsViewModelFactory _receiversConfigsViewModelFactory;


    public ReceiversConfigsViewModel(
        ObservableCollection<ReceiverConfigViewModel> receiverConfigViewModels,
        ReceiverConfigWindowFactory receiverConfigWindowFactory,
        IServiceBusHelperLogger logger,
        ReceivedMessageFormatter receivedMessageReceivedMessageFormatter,
        ServiceBusMessageReceiverFactory serviceBusMessageReceiverFactory,
        MessagePropertiesWindowProxyFactory messagePropertiesWindowProxyFactory,
        DeadLetterMessagePropertiesWindowProxyFactory deadLetterMessagePropertiesWindowProxyFactory, 
        ReceiversConfigsViewModelFactory receiversConfigsViewModelFactory)
    {
        _receiverConfigViewModels = receiverConfigViewModels;
        _receiverConfigWindowFactory = receiverConfigWindowFactory;
        _logger = logger;
        _receivedMessageFormatter = receivedMessageReceivedMessageFormatter;
        _serviceBusMessageReceiverFactory = serviceBusMessageReceiverFactory;
        _messagePropertiesWindowProxyFactory = messagePropertiesWindowProxyFactory;
        _deadLetterMessagePropertiesWindowProxyFactory = deadLetterMessagePropertiesWindowProxyFactory;
        _receiversConfigsViewModelFactory = receiversConfigsViewModelFactory;
    }

    public IList<ReceiverConfigViewModel> ReceiversConfigsVMs => _receiverConfigViewModels;

    public ReceiverConfigViewModel CurrentSelectedConfigModelItem
    {
        get => _currentSelectedItem;
        set
        {
            if (value == _currentSelectedItem) return;
            _currentSelectedItem = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void AddNewForModelItem(ReceiverConfigModel item)
    {
        var windowForReceiverConfig = _receiverConfigWindowFactory.CreateWindowForConfig();
        var callbacks = new ReceiverSeparateWindowManagementCallbacks()
        {
            OnAttachAction = () => { windowForReceiverConfig.HideWindow(); },
            OnDetachAction = () => { windowForReceiverConfig.ShowAsDetachedWindow(); }
        };

        var viewModel = new ReceiverConfigViewModel(
            item,
            _receivedMessageFormatter,
            _serviceBusMessageReceiverFactory.Create(),
            _messagePropertiesWindowProxyFactory.Create(),
            _deadLetterMessagePropertiesWindowProxyFactory.Create(),
            callbacks);


        windowForReceiverConfig.SetDataContext(viewModel);
        ReceiversConfigsVMs.Add(viewModel);
        _windowsForItems.Add(viewModel, windowForReceiverConfig);
    }

    public void AddNew()
    {
        var item = _receiversConfigsViewModelFactory.Create();
        AddNewForModelItem(item);
    }

    public void Remove(ReceiverConfigViewModel item)
    {
        _windowsForItems[item].DeleteWindow();
        _receiverConfigViewModels.Remove(item);
        _windowsForItems.Remove(item);
    }

    public void MoveConfigUp(ReceiverConfigViewModel item)
    {
        if (!CanMoveUp(item)) return;
        var oldIndex = _receiverConfigViewModels.IndexOf(item);
        _receiverConfigViewModels.Move(oldIndex, oldIndex-1);
    }

    public void MoveConfigDown(ReceiverConfigViewModel item)
    {
        if (!CanMoveDown(item)) return;
        var oldIndex = _receiverConfigViewModels.IndexOf(item);
        _receiverConfigViewModels.Move(oldIndex, oldIndex+1);
    }

    public bool CanMoveDown(ReceiverConfigViewModel item)
    {
        if (item is null) return false;
        return _receiverConfigViewModels.IndexOf(item) < _receiverConfigViewModels.Count-1;
    }

    public bool CanMoveUp(ReceiverConfigViewModel item)
    {
        if (item is null) return false;
        return _receiverConfigViewModels.IndexOf(item) > 0;
    }
}

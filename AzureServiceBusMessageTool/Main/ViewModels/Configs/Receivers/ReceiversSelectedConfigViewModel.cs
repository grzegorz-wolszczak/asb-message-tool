using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Main.Commands;
using Main.ConfigsGuiMetadata;
using Main.Models;
using Main.Utils;
using Main.Windows.DeadLetterMessage;

namespace Main.ViewModels.Configs.Receivers;

public sealed class ReceiversSelectedConfigViewModel : INotifyPropertyChanged
{
    private ReceiverConfigViewModel _currentSelectedItem;
    private bool _isReceiverConfigTabSelected;
    private bool _isEmbeddedSenderConfigUserControlForEditingEnabled;
    private ServiceBusMessageReceiverFactory _serviceBusMessageReceiverFactory;
    private readonly MessagePropertiesWindowProxyFactory _messagePropertiesWindowProxyFactory;
    private DeadLetterMessagePropertiesWindowProxyFactory _deadLetterMessagePropertiesWindowProxyFactory;
    private readonly ReceivedMessageFormatter _receivedMessageFormatter;

    public bool IsReceiverConfigTabSelected
    {
        get => _isReceiverConfigTabSelected;
        set
        {
            if (value == _isReceiverConfigTabSelected) return;
            _isReceiverConfigTabSelected = value;
            OnPropertyChanged();
        }
    }

    public void AddConfigs(List<ReceiverConfigModel> settingsReceiversConfig)
    {
        settingsReceiversConfig.ForEach(AddNewConfig);
    }

    public ReceiversSelectedConfigViewModel(
        ServiceBusMessageReceiverFactory serviceBusMessageReceiverFactory,
        MessagePropertiesWindowProxyFactory messagePropertiesWindowProxyFactory,
        DeadLetterMessagePropertiesWindowProxyFactory deadLetterMessagePropertiesWindowProxyFactory,
        ReceivedMessageFormatter receivedMessageFormatter)
    {
        _serviceBusMessageReceiverFactory = serviceBusMessageReceiverFactory;
        _messagePropertiesWindowProxyFactory = messagePropertiesWindowProxyFactory;
        _deadLetterMessagePropertiesWindowProxyFactory = deadLetterMessagePropertiesWindowProxyFactory;
        _receiverConfigElementsGuiMetadataManager = new ReceiverConfigElementsGuiMetadataManager();
        _receivedMessageFormatter = receivedMessageFormatter;
        AddReceiverConfigCommand = new DelegateCommand(_ =>
        {
            var newConfig = new ReceiverConfigModel
            {
                ConfigId = Guid.NewGuid().ToString(),
                ConfigName = "<config name not set>",
            };

            // copy one field that is secret
            if (_currentSelectedItem is { Item: { } })
            {
                newConfig.ServiceBusConnectionString = _currentSelectedItem.Item.ServiceBusConnectionString;
            }

            AddNewConfig(newConfig);
        });

        DeleteReceiverConfigCommand = new DelegateCommand(_ =>
            {
                _receiverConfigElementsGuiMetadataManager.Delete(CurrentSelectedConfigModelItem.ViewModelWrapper);
                ReceiversConfigs.Remove(CurrentSelectedConfigModelItem);
            },
            _ => CurrentSelectedConfigModelItem != null);
        ;
    }

    private void AddNewConfig(ReceiverConfigModel newConfig)
    {
        var viewModel = new ReceiverConfigViewModel(
            _receiverConfigElementsGuiMetadataManager,
            _messagePropertiesWindowProxyFactory.Create(),
            _deadLetterMessagePropertiesWindowProxyFactory.Create(),
            _serviceBusMessageReceiverFactory.Create(),
            _receivedMessageFormatter)
        {
            Item = newConfig
        };
        _receiverConfigElementsGuiMetadataManager.Add(viewModel.ViewModelWrapper);
        ReceiversConfigs.Add(viewModel);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private readonly IList<ReceiverConfigViewModel> _receiversConfigs = new ObservableCollection<ReceiverConfigViewModel>();
    private ReceiverConfigElementsGuiMetadataManager _receiverConfigElementsGuiMetadataManager;
    public IList<ReceiverConfigViewModel> ReceiversConfigs => _receiversConfigs;

    public ReceiverConfigViewModel CurrentSelectedConfigModelItem
    {
        get => _currentSelectedItem;
        set
        {
            if (value == _currentSelectedItem) return;
            _currentSelectedItem = value;
            IsEmbeddedSenderConfigUserControlForEditingEnabled = _currentSelectedItem != null;

            OnPropertyChanged();
        }
    }



    public bool IsEmbeddedSenderConfigUserControlForEditingEnabled
    {
        get => _isEmbeddedSenderConfigUserControlForEditingEnabled;
        set
        {
            if (value == _isEmbeddedSenderConfigUserControlForEditingEnabled) return;
            _isEmbeddedSenderConfigUserControlForEditingEnabled = value;

            OnPropertyChanged();
        }
    }

    public ICommand AddReceiverConfigCommand { get; }
    public ICommand DeleteReceiverConfigCommand { get; }
}

public class DeadLetterMessagePropertiesWindowProxyFactory
{
    public IDeadLetterMessagePropertiesWindowProxy Create()
    {
        return new DeadLetterMessagePropertiesWindowProxy();
    }
}

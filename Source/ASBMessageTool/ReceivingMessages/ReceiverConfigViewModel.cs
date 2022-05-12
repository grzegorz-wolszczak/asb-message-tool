using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.Application.Config;

namespace ASBMessageTool.ReceivingMessages;

public enum ReceiverEnumStatus
{
    Idle = 0,
    StoppedOnError,
    Listening,
    Initializing
}

public sealed class ReceiverConfigViewModel : INotifyPropertyChanged
{
    private bool _isEditingConfigurationEnabled = true;
    private readonly ReceivedMessageFormatter _receivedMessageFormatter;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private ReceiverConfigModel _modelItem;
    private bool _isDetached = false;
    private string _receivedMessagesContent;
    private ReceiverEnumStatus _receiverEnumStatus = ReceiverEnumStatus.Idle;
    private string _receiverStatusTextText = ReceiverEnumStatus.Idle.ToString();
    private IServiceBusMessageReceiver _serviceBusMessageReceiver;
    private IMessageApplicationPropertiesWindowProxy _messageApplicationPropertiesWindowProxy;
    private readonly IDeadLetterMessagePropertiesWindowProxy _deadLetterMessagePropertiesWindowProxy;
    private readonly SeparateWindowManagementCallbacks _separateWindowManagementCallbacks;
    private ConfigEditingEnabler _configEditorEnabler;
    private readonly IReceiverSettingsValidator _receiverSettingsValidator;

    public ReceiverConfigViewModel(ReceiverConfigModel item,
        SeparateWindowManagementCallbacks separateWindowManagementCallbacks,
        IServiceBusMessageReceiver serviceBusMessageReceiver,
        IMessageApplicationPropertiesWindowProxy messageApplicationPropertiesWindowProxy,
        IDeadLetterMessagePropertiesWindowProxy deadLetterMessagePropertiesWindowProxy,
        ReceivedMessageFormatter formatter, 
        IReceiverSettingsValidator receiverSettingsValidator, 
        IInGuiThreadActionCaller inGuiThreadActionCaller)
    {
        _receivedMessageFormatter = formatter;
        _receiverSettingsValidator = receiverSettingsValidator;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _serviceBusMessageReceiver = serviceBusMessageReceiver;
        _messageApplicationPropertiesWindowProxy = messageApplicationPropertiesWindowProxy;
        _deadLetterMessagePropertiesWindowProxy = deadLetterMessagePropertiesWindowProxy;
        _separateWindowManagementCallbacks = separateWindowManagementCallbacks;
        ModelItem = item;
        
        _configEditorEnabler = new ConfigEditingEnabler(value => { IsEditingConfigurationEnabled = value; });
        
        StartMessageReceiveCommand = new StartMessageReceiveCommand(_serviceBusMessageReceiver,
            _inGuiThreadActionCaller,
            serviceBusReceiverProviderFunc: GetServiceBusReceiverSettings,
            onMessageReceived: AppendReceivedMessageToOutput,
            onReceiverStarted: SetReceiverListeningStatus,
            onReceiverFailure: SetReceiverStoppedOnErrorStatus,
            onReceiverStopped: SetSetReceiverIdleStatus,
            onReceiverInitializing: SetReceiverInitializingStatus
        );

        ValidateConfigurationCommand = new ValidateReceiverConfigurationCommand(
            onValidationStartedAction: ()=>{ _configEditorEnabler.SetConfigValidationStarted();},
            onValidationFinishedAction:() => { _configEditorEnabler.SetConfigValidationFinished();},
            _inGuiThreadActionCaller,
            GetServiceBusReceiverSettings,
            _receiverSettingsValidator);
        
        StopMessageReceiveCommand = new DelegateCommand(_ => { _serviceBusMessageReceiver.Stop(); },
            _ => !StartMessageReceiveCommand.CanExecute(default));

        ShowAbandonMessageOverriddenPropertiesWindowCommand = new DelegateCommand(_ =>
        {
            _messageApplicationPropertiesWindowProxy.ShowDialog(new SbMessageApplicationPropertiesViewModel(
                _modelItem.AbandonMessageApplicationOverridenProperties
            ));
        });

        ShowDeadLetterMessageOverriddenPropertiesWindowCommand = new DelegateCommand(_ =>
        {
            _deadLetterMessagePropertiesWindowProxy.ShowDialog(new DeadLetterMessagePropertiesViewModel(_modelItem));

        });

        AttachToPanelCommand = new DelegateCommand((_) =>
        {
            IsContentDetached = false;
            _separateWindowManagementCallbacks.OnAttachAction();
        });
        DetachFromPanelCommand = new DelegateCommand((_) =>
        {
            IsContentDetached = true;
            _separateWindowManagementCallbacks.OnDetachAction();
        });

        ClearMessageContentCommand = new DelegateCommand(_ => { ReceivedMessagesContent = string.Empty; });
    }

    private ServiceBusReceiverSettings GetServiceBusReceiverSettings()
    {
        return new ServiceBusReceiverSettings
        {
            ConfigName = ModelItem.ConfigName,
            ConnectionString = ModelItem.ServiceBusConnectionString,
            SubscriptionName = ModelItem.InputTopicSubscriptionName,
            TopicName = ModelItem.InputTopicName,
            IsDeadLetterQueue = ModelItem.IsAttachedToDeadLetterSubqueue,
            MessageReceiveDelayPeriod = StaticConfig.NextMessageReceiveDelayTimeSpan, // todo: support this from gui,
            OnMessageReceiveEnumAction = ModelItem.OnMessageReceiveAction,
            AbandonMessageOverriddenApplicationProperties = ModelItem.AbandonMessageApplicationOverridenProperties,
            DeadLetterMessageOverriddenApplicationProperties = ModelItem.DeadLetterMessageApplicationOverridenProperties,
            DeadLetterMessageFields = ModelItem.DeadLetterMessageFields,
            DeadLetterMessageFieldsOverrideType = ModelItem.DeadLetterMessageFieldsOverrideType,
            ShouldShowOnlyMessageBodyAsJson = ModelItem.ShouldShowOnlyBodyAsJson,
            ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter = ModelItem.ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter,
            ReceiverQueueName = ModelItem.ReceiverQueueName,
            ReceiverDataSourceType = ModelItem.ReceiverDataSourceType
        };
    }

    public ICommand StartMessageReceiveCommand { get; }
    public ICommand StopMessageReceiveCommand { get; }

    public ICommand ShowAbandonMessageOverriddenPropertiesWindowCommand { get; }
    public ICommand ShowDeadLetterMessageOverriddenPropertiesWindowCommand { get; }

    public ICommand DetachFromPanelCommand { get; }
    public ICommand AttachToPanelCommand { get; }

    public ICommand ClearMessageContentCommand { get; }
    
    public ICommand ValidateConfigurationCommand { get; }


    public bool IsContentDetached
    {
        get => _isDetached;
        set
        {
            if (value == _isDetached) return;
            _isDetached = value;
            OnPropertyChanged();
        }
    }

    public ReceiverConfigModel ModelItem
    {
        get => _modelItem;
        set
        {
            if (value == _modelItem) return;
            _modelItem = value;
            OnPropertyChanged();
        }
    }

    private void SetSetReceiverIdleStatus()
    {
        _configEditorEnabler.ExternalTaskThatBlocksConfigurationEditingFinished();
        ReceiverStatusText = "Idle";
        ReceiverEnumStatus = ReceiverEnumStatus.Idle;
    }

    private void SetReceiverStoppedOnErrorStatus(Exception exception)
    {
        _configEditorEnabler.ExternalTaskThatBlocksConfigurationEditingFinished();
        ReceiverStatusText = "Stopped because of error";
        ReceiverEnumStatus = ReceiverEnumStatus.StoppedOnError;
    }

    private void SetReceiverListeningStatus()
    {
        _configEditorEnabler.ExternalTaskThatBlocksConfigurationEditingStarted();
        ReceiverStatusText = "Listening...";
        ReceiverEnumStatus = ReceiverEnumStatus.Listening;
    }

    private void SetReceiverInitializingStatus()
    {
        ReceiverStatusText = "Initializing...";
        ReceiverEnumStatus = ReceiverEnumStatus.Initializing;
    }

    public string ReceivedMessagesContent
    {
        get => _receivedMessagesContent;
        set
        {
            if (value == _receivedMessagesContent) return;
            _receivedMessagesContent = value;
            OnPropertyChanged();
        }
    }

    private void AppendReceivedMessageToOutput(ReceivedMessage msg)
    {
        var msgBody = _receivedMessageFormatter.Format(
            msg.OriginalMessage,
            ModelItem.ShouldShowOnlyBodyAsJson,
            ModelItem.ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter);
        ReceivedMessagesContent += $"{TimeUtils.GetShortTimestamp()} received message: \n" +
                                   $"{msgBody}" +
                                   "\n----------------------------------\n";
    }


    public ReceiverEnumStatus ReceiverEnumStatus
    {
        get => _receiverEnumStatus;
        set
        {
            if (value == _receiverEnumStatus) return;
            _receiverEnumStatus = value;
            OnPropertyChanged();
        }
    }

    public string ReceiverStatusText
    {
        get => _receiverStatusTextText;
        set
        {
            if (value == _receiverStatusTextText) return;
            _receiverStatusTextText = value;
            OnPropertyChanged();
        }
    }

    public bool IsEditingConfigurationEnabled
    {
        get => _isEditingConfigurationEnabled;
        set
        {
            if (value == _isEditingConfigurationEnabled) return;
            _isEditingConfigurationEnabled = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

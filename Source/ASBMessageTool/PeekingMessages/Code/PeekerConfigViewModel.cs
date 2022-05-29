using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.ReceivingMessages.Code;
using JetBrains.Annotations;

namespace ASBMessageTool.PeekingMessages.Code;

public enum PeekerEnumStatus
{
    Idle = 0,
    StoppedOnError,
    Peeking,
    Initializing
}

public sealed class PeekerConfigViewModel : INotifyPropertyChanged
{
    private readonly SeparateWindowManagementCallbacks _separateWindowManagementCallbacks;
    private readonly IServiceBusMessagePeeker _messagePeeker;
    private readonly ReceivedMessageFormatter _receivedMessageFormatter;
    private readonly IPeekerSettingsValidator _peekerSettingsValidator;
    private readonly InGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly OperationSystemServices _operationSystemServices; 
    private string _receivedMessagesContent;
    private string _peekerStatusText = PeekerEnumStatus.Idle.ToString();
    private PeekerEnumStatus _peekerEnumStatus = PeekerEnumStatus.Idle;
    private bool _isEditingConfigurationEnabled = true;
    private bool _isConfigurationViewExpanded = true;
    private ConfigEditingEnabler _configEditorEnabler;

    private bool _isDetached = false;
    private PeekerConfigModel _modelItem;


    public PeekerConfigViewModel(
        PeekerConfigModel configModel,
        SeparateWindowManagementCallbacks separateWindowManagementCallbacks,
        IServiceBusMessagePeeker messagePeeker,
        ReceivedMessageFormatter receivedMessageFormatter,
        IPeekerSettingsValidator peekerSettingsValidator,
        InGuiThreadActionCaller inGuiThreadActionCaller,
        OperationSystemServices operationSystemServices)
    {
        _separateWindowManagementCallbacks = separateWindowManagementCallbacks;
        _messagePeeker = messagePeeker;
        _receivedMessageFormatter = receivedMessageFormatter;
        _peekerSettingsValidator = peekerSettingsValidator;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _operationSystemServices = operationSystemServices;
        ModelItem = configModel;

        _configEditorEnabler = new ConfigEditingEnabler(value => { IsEditingConfigurationEnabled = value; });

        DetachFromPanelCommand = new DelegateCommand((_) =>
        {
            IsContentDetached = true;
            _separateWindowManagementCallbacks.OnDetachAction();
        });

        AttachToPanelCommand = new DelegateCommand((_) =>
        {
            IsContentDetached = false;
            _separateWindowManagementCallbacks.OnAttachAction();
        });

        ValidateConfigurationCommand = new ValidatePeekerConfigurationCommand(
            () => { _configEditorEnabler.SetConfigValidationStarted(); },
            () => { _configEditorEnabler.SetConfigValidationFinished(); },
            _inGuiThreadActionCaller,
            GetServiceBusPeekerSettings,
            _peekerSettingsValidator);

        PeekMessagesCommand = new PeekMessagesCommand(
            _messagePeeker,
            _inGuiThreadActionCaller,
            GetServiceBusPeekerSettings,
            AppendPeekedMessageToOutput,
            SetPeekerListeningStatus,
            SetPeekerStopOnErrorStatus,
            SetPeekerIdleStatus,
            SetPeekerInitializingStatus);

        ClearMessageContentCommand = new DelegateCommand(_ => { ReceivedMessagesContent = string.Empty; });
        
        CopySenderConnectionStringToClipboard = new DelegateCommand(_ =>
        {
            _operationSystemServices.SetClipboardText(_modelItem.ServiceBusConnectionString);
        });

        StopPeekingCommand = new DelegateCommand(_ => _messagePeeker.Stop(), _ =>  !PeekMessagesCommand.CanExecute(default));
    }

    private void SetPeekerInitializingStatus()
    {
        PeekerStatusText = "Initializing...";
        PeekerEnumStatus = PeekerEnumStatus.Initializing;
    }

    private void SetPeekerIdleStatus()
    {
        _configEditorEnabler.ExternalTaskThatBlocksConfigurationEditingFinished();
        PeekerStatusText = "Idle";
        PeekerEnumStatus = PeekerEnumStatus.Idle;
    }

    private void AppendPeekedMessageToOutput(List<PeekedMessage> messages)
    {
        var builder = new StringBuilder();
        foreach (var msg in messages)
        {
            var msgBody = _receivedMessageFormatter.Format(
                msg.OriginalMessage,
                ModelItem.ShouldShowOnlyBodyAsJson,
                ModelItem.ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter);
            var contentToAppend = $"{TimeUtils.GetShortTimestamp()} received message: \n" +
                                  $"{msgBody}" +
                                  "\n----------------------------------\n";
            builder.AppendLine(contentToAppend);
        }

        builder.AppendLine($"Total messages in queue: {messages.Count}");
        ReceivedMessagesContent = builder.ToString();
    }

    private void SetPeekerListeningStatus()
    {
        _configEditorEnabler.ExternalTaskThatBlocksConfigurationEditingStarted();
        PeekerStatusText = "Peeking...";
        PeekerEnumStatus = PeekerEnumStatus.Peeking;
    }

    private void SetPeekerStopOnErrorStatus(Exception exception)
    {
        _configEditorEnabler.ExternalTaskThatBlocksConfigurationEditingFinished();
        PeekerStatusText = "Stopped because of error";
        PeekerEnumStatus = PeekerEnumStatus.StoppedOnError;
    }

    private ServiceBusPeekerSettings GetServiceBusPeekerSettings()
    {
        return new ServiceBusPeekerSettings()
        {
            ConfigName = ModelItem.ConfigName,
            ConnectionString = ModelItem.ServiceBusConnectionString,
            TopicName = ModelItem.InputTopicName,
            SubscriptionName = ModelItem.InputTopicSubscriptionName,
            ReceiverQueueName = ModelItem.ReceiverQueueName,
            IsDeadLetterQueue = ModelItem.IsAttachedToDeadLetterSubqueue,
            ReceiverDataSourceType = ModelItem.ReceiverDataSourceType,
            ShouldShowOnlyMessageBodyAsJson = ModelItem.ShouldShowOnlyBodyAsJson,
            ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter = ModelItem.ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter
        };
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

    
    [UsedImplicitly]
    public bool IsConfigurationViewExpanded
    {
        get => _isConfigurationViewExpanded;
        set
        {
            if (value == _isConfigurationViewExpanded) return;
            _isConfigurationViewExpanded = value;
            OnPropertyChanged();
        }
    }
    
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

    public PeekerEnumStatus PeekerEnumStatus
    {
        get => _peekerEnumStatus;
        set
        {
            if (value == _peekerEnumStatus) return;
            _peekerEnumStatus = value;
            OnPropertyChanged();
        }
    }

    public PeekerConfigModel ModelItem
    {
        get => _modelItem;
        set
        {
            if (value == _modelItem) return;
            _modelItem = value;
            OnPropertyChanged();
        }
    }

    public string PeekerStatusText
    {
        get => _peekerStatusText;
        set
        {
            if (value == _peekerStatusText) return;
            _peekerStatusText = value;
            OnPropertyChanged();
        }
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

    public ICommand DetachFromPanelCommand { get; }
    public ICommand AttachToPanelCommand { get; }

    public ICommand ValidateConfigurationCommand { get; }

    public ICommand PeekMessagesCommand { get; }

    public ICommand ClearMessageContentCommand { get; }
    public ICommand CopySenderConnectionStringToClipboard { get; }
    public ICommand StopPeekingCommand { get; }
    

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ASBMessageTool.Application;
using ASBMessageTool.Application.Logging;
using ASBMessageTool.Model;
using Core.Maybe;
using ICSharpCode.AvalonEdit.Document;
using JetBrains.Annotations;

namespace ASBMessageTool.SendingMessages;

public enum MessageSendingStatus
{
    Idle,
    Sending,
    Success,
    Error
}



public sealed class SenderConfigViewModel : INotifyPropertyChanged
{
    private record LastMessageSendingError(string Message);
    
    private SenderConfigModel _modelItem;
    private readonly SeparateWindowManagementCallbacks _separateWindowManagementCallbacks;
    private readonly ISenderMessagePropertiesWindowProxy _senderMessagePropertiesWindowProxy;
    private readonly IMessageSender _messageSender;
    private readonly IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly IServiceBusHelperLogger _logger;
    private bool _isDetached = false;
    private string _lastSendStatusText = "N/A";
    private MessageSendingStatus _lastSendStatus = MessageSendingStatus.Idle;
    private TextDocument _msgToSendDocument = new("");
    private ISenderSettingsValidator _senderSettingsValidator;
    private readonly IOperationSystemServices _operationSystemServices;
    private bool _isEditingConfigurationEnabled = true;
    private Maybe<LastMessageSendingError> _lastMessageSendingError = Maybe<LastMessageSendingError>.Nothing;
    private ConfigEditingEnabler _configEditorEnabler;


    public SenderConfigViewModel(SenderConfigModel modelItem,
        SeparateWindowManagementCallbacks separateWindowManagementCallbacks,
        ISenderMessagePropertiesWindowProxy senderMessagePropertiesWindowProxy,
        IMessageSender messageSender,
        IInGuiThreadActionCaller inGuiThreadActionCaller,
        IServiceBusHelperLogger logger,
        ISenderSettingsValidator senderSettingsValidator, 
        IOperationSystemServices operationSystemServices)
    {
        ModelItem = modelItem; // must use Property instead of backing field because Document.Text must be set
        _separateWindowManagementCallbacks = separateWindowManagementCallbacks;
        _senderMessagePropertiesWindowProxy = senderMessagePropertiesWindowProxy;
        _messageSender = messageSender;
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _logger = logger;
        _senderSettingsValidator = senderSettingsValidator;
        _operationSystemServices = operationSystemServices;

        _configEditorEnabler = new ConfigEditingEnabler(value => { IsEditingConfigurationEnabled = value; });

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

        ShowPropertiesWindowCommand = new DelegateCommand(_ =>
        {
            _senderMessagePropertiesWindowProxy.ShowDialog(new SbMessageFieldsViewModel(
                _modelItem.ApplicationProperties,
                _modelItem.MessageFields));
        });

        ValidateConfigurationCommand = new ValidateSenderConfigurationCommand(
            onValidationStartedAction: ()=>{ _configEditorEnabler.SetConfigValidationStarted();},
            onValidationFinishedAction:() => { _configEditorEnabler.SetConfigValidationFinished();},
        inGuiThreadActionCaller,
            GetServiceBusMessageSendData,
            _senderSettingsValidator);

        
        SendMessageCommand = new SendMessageCommand(_messageSender,
            onStartSendingAction: IndicateSendingInStarted,
            onFinishedSendingAction: IndicateSendingFinished,
            msgProviderFunc: GetServiceBusMessageSendData,
            onErrorWhileSendingHappenedAction: IndicateErrorHappenedWhenSending,
            onSendingErrorHappened: IndicateUnexpectedExceptionFinished,
            _inGuiThreadActionCaller);

        CopySenderConnectionStringToClipboard = new DelegateCommand(_ =>
        {
            _operationSystemServices.SetClipboardText(_modelItem.ServiceBusConnectionString);
        });
    }

    private void IndicateErrorHappenedWhenSending(MessageSendErrorInfo messageSendErrorInfo)
    {
        SetLastSendingError(messageSendErrorInfo.Message);
    }

    private void IndicateUnexpectedExceptionFinished(Exception exception)
    {
        _logger.LogException("While sending message exception happened: ", exception);
        SetLastSendingError("Unexpected error, see logs for details.");
    }

    private void IndicateSendingFinished()
    {
        _configEditorEnabler.ExternalTaskThatBlocksConfigurationEditingFinished();
        if (_lastMessageSendingError.IsSomething())
        {
            MessageSendingStatus = MessageSendingStatus.Error;
            SetLastSendStatusMessage(_lastMessageSendingError.Value().Message);
        }
        else
        {
            SetLastSendStatusMessage("Message send successfully");
            MessageSendingStatus = MessageSendingStatus.Success;    
        }
    }

    private void IndicateSendingInStarted()
    {
        _lastMessageSendingError = Maybe<LastMessageSendingError>.Nothing;
        _configEditorEnabler.ExternalTaskThatBlocksConfigurationEditingStarted();
        SetLastSendStatusMessage("Sending...");
        MessageSendingStatus = MessageSendingStatus.Sending;
    }

    private void SetLastSendingError(string msg)
    {
        _lastMessageSendingError = new LastMessageSendingError(msg).ToMaybe();
    }
    
    private void SetLastSendStatusMessage(string msg)
    {
        var status = $"{TimeUtils.GetShortTimestamp()} {msg}";
        MessageSendingStatusText = status;
    }

    
    private ServiceBusMessageSendData GetServiceBusMessageSendData()
    {
        return new ServiceBusMessageSendData
        {
            ConnectionString = ModelItem.ServiceBusConnectionString,
            MsgBody = ModelItem.MsgBody,
            QueueOrTopicName = ModelItem.OutputTopicName,
            Fields = ModelItem.MessageFields,
            ApplicationProperties = ModelItem.ApplicationProperties,
            ConfigName = ModelItem.ConfigName
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
    public ICommand AttachToPanelCommand { get; }
    public ICommand DetachFromPanelCommand { get; }
    public ICommand SendMessageCommand { get; }
    public ICommand ShowPropertiesWindowCommand { get; }
    public ICommand ValidateConfigurationCommand { get; }
    
    public ICommand CopySenderConnectionStringToClipboard { get; }
    
    public event PropertyChangedEventHandler PropertyChanged;
    

    public SenderConfigModel ModelItem
    {
        get => _modelItem;
        set
        {
            if (value == _modelItem) return;
            _modelItem = value;
            if (_modelItem is not null && _msgToSendDocument is not null)
            {
                _msgToSendDocument.Text = _modelItem.MsgBody;
            }

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

    [UsedImplicitly]
    public TextDocument AvalonTextDocument
    {
        get => _msgToSendDocument;
        set
        {
            if (value == _msgToSendDocument) return;
            _msgToSendDocument = value;
            OnPropertyChanged();
        }
    }

    public string MessageSendingStatusText
    {
        get => _lastSendStatusText;
        set
        {
            if (value == _lastSendStatusText) return;
            _lastSendStatusText = value;
            OnPropertyChanged();
        }
    }

    public MessageSendingStatus MessageSendingStatus
    {
        get => _lastSendStatus;
        set
        {
            if (value == _lastSendStatus) return;
            _lastSendStatus = value;
            OnPropertyChanged();
        }
    }


    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

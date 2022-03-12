using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
using Main.Application.Logging;
using Main.Commands;
using Main.Models;
using Main.Utils;
using Main.Windows.MessageToSend;

namespace Main.ViewModels.Configs.Senders;

public enum LastSendStatus
{
    Idle,
    Success,
    Error
}

public class SenderConfigViewModel : INotifyPropertyChanged
{
    private readonly ISenderConfigWindowDetacher _senderConfigWindowDetacher;
    private readonly IMessageSender _messageSender;
    private SenderConfigModel _item;
    private bool _isEmbeddedInsideRightPanel = true;


    private string _lastSendStatusText = "N/A";
    private LastSendStatus _lastSendStatus = LastSendStatus.Idle;


    //private string _messageToSendStringContent;
    private IInGuiThreadActionCaller _inGuiThreadActionCaller;
    private readonly IServiceBusHelperLogger _logger;
    private TextDocument _msgToSendDocument = new TextDocument("");

    public readonly SenderConfigViewModelWrapper ViewModelWrapper;
    private ISenderMessagePropertiesWindowProxy _senderMessagePropertiesWindowProxy;

    public ICommand DetachFromPanelCommand { get; }
    public ICommand SendMessageCommand { get; }
    public ICommand ShowPropertiesWindowCommand { get; }


    public SenderConfigViewModel(
        ISenderConfigWindowDetacher senderConfigWindowDetacher,
        IMessageSender messageSender,
        IInGuiThreadActionCaller inGuiThreadActionCaller,
        IServiceBusHelperLogger logger,
        ISenderMessagePropertiesWindowProxy senderMessagePropertiesWindowProxy)
    {
        _senderConfigWindowDetacher = senderConfigWindowDetacher;
        _messageSender = messageSender;
        ViewModelWrapper = new SenderConfigViewModelWrapper(this);

        DetachFromPanelCommand = new DelegateCommand(onExecuteMethod: _ => { _senderConfigWindowDetacher.DetachFromPanel(ViewModelWrapper); });
        _inGuiThreadActionCaller = inGuiThreadActionCaller;
        _logger = logger;

        _senderMessagePropertiesWindowProxy = senderMessagePropertiesWindowProxy;

        ShowPropertiesWindowCommand = new DelegateCommand(_ =>
        {
            _senderMessagePropertiesWindowProxy.ShowDialog(new SbMessageFieldsViewModel(
                _item.ApplicationProperties,
                _item.MessageFields));

        });

        SendMessageCommand = new SendServiceMessageCommand(_messageSender,
            msgProviderFunc: () =>
            {
                return new MessageToSendData
                {
                    ConnectionString = Item.ServiceBusConnectionString,
                    MsgBody = Item.MsgBody,
                    TopicName = Item.OutputTopicName,
                    Fields = Item.MessageFields,
                    ApplicationProperties = Item.ApplicationProperties
                };
            },
            onErrorAction: e => { SetLastSendStatusErrorMessage(e.Message); },
            onSuccessAction: statusMessage => { SetLastSendStatusSuccessMessage(statusMessage); },
            onUnexpectedExceptionAction: exception =>
            {
                _logger.LogException("While sending message exception happened: ", exception);
                SetLastSendStatusErrorMessage("Unexpected error, see logs for details.");
            },
            _inGuiThreadActionCaller);
    }

    private void SetLastSendStatusSuccessMessage(string statusMessage)
    {
        SetLastSendStatusMessage(statusMessage);
        LastSendStatus = LastSendStatus.Success;
    }

    private void SetLastSendStatusErrorMessage(string msg)
    {
        SetLastSendStatusMessage(msg);
        LastSendStatus = LastSendStatus.Error;
    }

    public TextDocument TextDocument
    {
        get => _msgToSendDocument;
        set
        {

            if (value == _msgToSendDocument) return;
            _msgToSendDocument = value;
            OnPropertyChanged();
        }
    }

    public LastSendStatus LastSendStatus
    {
        get => _lastSendStatus;
        set
        {

            if (value == _lastSendStatus) return;
            _lastSendStatus = value;
            OnPropertyChanged();
        }
    }

    private void SetLastSendStatusMessage(string msg)
    {
        var status = $"{TimeUtils.GetShortTimestamp()} {msg}";
        LastSendStatusText = status;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public SenderConfigModel Item
    {
        get => _item;
        set
        {
            if (value == _item) return;
            _item = value;
            if (_item != null && _msgToSendDocument != null)
            {
#pragma warning disable CA1416
                _msgToSendDocument.Text = _item.MsgBody;
#pragma warning restore CA1416
            }

            OnPropertyChanged();
        }
    }

    public string LastSendStatusText
    {
        get => _lastSendStatusText;
        set
        {
            if (value == _lastSendStatusText) return;
            _lastSendStatusText = value;
            OnPropertyChanged();
        }
    }


    public bool IsEmbeddedInsideRightPanel
    {
        get => _isEmbeddedInsideRightPanel;
        set
        {
            if (value == _isEmbeddedInsideRightPanel) return;
            _isEmbeddedInsideRightPanel = value;
            OnPropertyChanged();
        }
    }
}

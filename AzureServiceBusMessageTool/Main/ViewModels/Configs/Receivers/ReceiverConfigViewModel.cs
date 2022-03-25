using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Main.Application;
using Main.Commands;
using Main.Models;
using Main.Utils;
using Main.Windows.ApplicationProperties;
using Main.Windows.DeadLetterMessage;

namespace Main.ViewModels.Configs.Receivers;

public enum ReceiverEnumStatus
{
    Idle =0,
    StoppedOnError,
    Listening,
    Initializing
}

public class ReceiverConfigViewModel : INotifyPropertyChanged
{
    private readonly IReceiverConfigWindowDetacher _receiverConfigWindowDetacher;
    private ReceiverEnumStatus _receiverEnumStatus = ReceiverEnumStatus.Idle;

    private ReceiverConfigModel _item;
    private bool _isEmbeddedInsideRightPanel = true;
    private string _receivedMessagesContent;
    private string _receiverStatusTextText = "Idle";

    public ICommand DetachFromPanelCommand { get; }
    public ICommand StartMessageReceiveCommand { get; }
    public ICommand StopMessageReceiveCommand { get; }
    public ICommand ClearMessageContentCommand { get; }
    public ICommand ShowAbandonMessageOverriddenPropertiesWindowCommand { get; }
    public ICommand ShowDeadLetterMessageOverriddenPropertiesWindowCommand { get; }

    public readonly ReceiverConfigViewModelWrapper ViewModelWrapper;
    private IMessageApplicationPropertiesWindowProxy _messageApplicationPropertiesWindowProxy;
    private readonly IDeadLetterMessagePropertiesWindowProxy _deadLetterMessagePropertiesWindowProxy;
    private IServiceBusMessageReceiver _serviceBusMessageReceiver;
    private readonly ReceivedMessageFormatter _receivedMessageFormatter;

    public ReceiverConfigViewModel(IReceiverConfigWindowDetacher receiverConfigWindowDetacher,
        IMessageApplicationPropertiesWindowProxy messageApplicationPropertiesWindowProxy,
        IDeadLetterMessagePropertiesWindowProxy deadLetterMessagePropertiesWindowProxy,
        IServiceBusMessageReceiver serviceBusMessageReceiver,
        ReceivedMessageFormatter receivedMessageFormatter)
    {
        _receivedMessageFormatter = receivedMessageFormatter;
        _messageApplicationPropertiesWindowProxy = messageApplicationPropertiesWindowProxy;
        _deadLetterMessagePropertiesWindowProxy = deadLetterMessagePropertiesWindowProxy;
        _receiverConfigWindowDetacher = receiverConfigWindowDetacher;

        ViewModelWrapper = new ReceiverConfigViewModelWrapper(this);

        DetachFromPanelCommand = new DelegateCommand(onExecuteMethod: _ => { _receiverConfigWindowDetacher.DetachFromPanel(ViewModelWrapper); });
        ClearMessageContentCommand = new DelegateCommand(_ => { ReceivedMessagesContent = string.Empty; });
        _serviceBusMessageReceiver = serviceBusMessageReceiver;


        StartMessageReceiveCommand = new StartMessageReceiveCommand(_serviceBusMessageReceiver,
            serviceBusReceiverProviderFunc: () => new ServiceBusReceiverSettings
            {
                ConfigName = Item.ConfigName,
                ConnectionString = Item.ServiceBusConnectionString,
                SubscriptionName = Item.InputTopicSubscriptionName,
                TopicName = Item.InputTopicName,
                IsDeadLetterQueue = Item.IsAttachedToDeadLetterSubqueue,
                MessageReceiveDelayPeriod = StaticConfig.NextMessageReceiveDelayTimeSpan, // todo: support this from gui,
                OnMessageReceiveEnumAction = Item.OnMessageReceiveAction,
                AbandonMessageOverriddenApplicationProperties = Item.AbandonMessageApplicationOverridenProperties,
                DeadLetterMessageOverriddenApplicationProperties = Item.DeadLetterMessageApplicationOverridenProperties,
                DeadLetterMessageFields = Item.DeadLetterMessageFields,
                DeadLetterMessageFieldsOverrideType = Item.DeadLetterMessageFieldsOverrideType,
                ShouldShowOnlyMessageBodyAsJson = Item.ShouldShowOnlyBodyAsJson,
                ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter = Item.ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter,
                ReceiverQueueName = Item.ReceiverQueueName,
                ReceiverDataSourceType = Item.ReceiverDataSourceType
            },
            onMessageReceived: AppendReceivedMessageToOutput,
            onReceiverStarted: SetReceiverListeningStatus,
            onReceiverFailure: SetReceiverStoppedOnErrorStatus,
            onReceiverStopped: SetSetReceiverIdleStatus,
            onReceiverInitializing: SetReceiverInitializingStatus
        );

        StopMessageReceiveCommand = new DelegateCommand(_ => { _serviceBusMessageReceiver.Stop(); },
            _ => !StartMessageReceiveCommand.CanExecute(default));

        ShowAbandonMessageOverriddenPropertiesWindowCommand = new DelegateCommand(_ =>
        {
            _messageApplicationPropertiesWindowProxy.ShowDialog(new SbMessageApplicationPropertiesViewModel(
                _item.AbandonMessageApplicationOverridenProperties
                ));
        });

        ShowDeadLetterMessageOverriddenPropertiesWindowCommand = new DelegateCommand(_ =>
        {
            _deadLetterMessagePropertiesWindowProxy.ShowDialog(
                new DeadLetterMessagePropertiesViewModel(_item));

        });
    }

    private void SetSetReceiverIdleStatus()
    {
        ReceiverStatusText = "Idle";
        ReceiverEnumStatus = ReceiverEnumStatus.Idle;
    }

    private void SetReceiverStoppedOnErrorStatus(Exception exception)
    {
        ReceiverStatusText = "Stopped because of error";
        ReceiverEnumStatus = ReceiverEnumStatus.StoppedOnError;
    }

    private void SetReceiverListeningStatus()
    {
        ReceiverStatusText = "Listening...";
        ReceiverEnumStatus = ReceiverEnumStatus.Listening;
    }

    private void SetReceiverInitializingStatus()
    {
        ReceiverStatusText = "Initializing...";
        ReceiverEnumStatus = ReceiverEnumStatus.Initializing;
    }

    private void AppendReceivedMessageToOutput(ReceivedMessage msg)
    {
        var msgBody = _receivedMessageFormatter.Format(
            msg.OriginalMessage,
            Item.ShouldShowOnlyBodyAsJson,
            Item.ShouldReplaceJsonSlashNSlashRSequencesWithNewLineCharacter);
        ReceivedMessagesContent += $"{TimeUtils.GetShortTimestamp()} received message: \n" +
                                   $"{msgBody}" +
                                   "\n----------------------------------\n";
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

    public ReceiverConfigModel Item
    {
        get => _item;
        set
        {
            if (value == _item) return;
            _item = value;
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

public interface IReceiverConfigWindowDetacher
{
    void DetachFromPanel(ReceiverConfigViewModelWrapper item);
}

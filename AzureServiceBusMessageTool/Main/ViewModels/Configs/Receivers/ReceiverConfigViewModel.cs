using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Main.Application;
using Main.Commands;
using Main.Models;
using Main.Utils;

namespace Main.ViewModels.Configs.Receivers
{

   public enum ReceiverEnumStatus
   {
      Idle =0,
      StoppedOnError,
      Listening
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

      public readonly ReceiverConfigViewModelWrapper ViewModelWrapper;
      private ServiceBusMessageReceiverFactory _messageReceiverFactory;

      public ReceiverConfigViewModel(
         IReceiverConfigWindowDetacher receiverConfigWindowDetacher,
         ServiceBusMessageReceiverFactory serviceBusMessageReceiverFactory)
      {
         _messageReceiverFactory = serviceBusMessageReceiverFactory;
         _receiverConfigWindowDetacher = receiverConfigWindowDetacher;

         ViewModelWrapper = new ReceiverConfigViewModelWrapper(this);

         DetachFromPanelCommand = new DelegateCommand(onExecuteMethod: _ => { _receiverConfigWindowDetacher.DetachFromPanel(ViewModelWrapper); });
         ClearMessageContentCommand = new DelegateCommand(_ => { ReceivedMessagesContent = string.Empty; });
         IServiceBusMessageReceiver serviceBusMessageReceiver = _messageReceiverFactory.Create();


         StartMessageReceiveCommand = new StartMessageReceiveCommand(serviceBusMessageReceiver,
            serviceBusReceiverProviderFunc: () =>
            {
               return new ServiceBusReceiverSettings()
               {
                  ConfigName = Item.ConfigName,
                  ConnectionString = Item.ServiceBusConnectionString,
                  SubscriptionName = Item.InputTopicSubscriptionName,
                  TopicName = Item.InputTopicName,
                  IsDeadLetterQueue = Item.IsAttachedToDeadLetterSubqueue,
                  NextMessageReceiveDelayPeriod = StaticConfig.NextMessageReceiveDelayTimeSpan // todo: support this from gui
               };
            },
            onMessageReceived: (msg) => { ReceivedMessagesContent += $"{FormatReceivedMessage(msg)}\n"; },
            onReceiverStarted: SetListeningReceiverStatus,
            onReceiverFailure: SetStoppedOnErrorReceiverStatus,
            onReceiverStopped: SetIdleReceiverStatus
         );

         StopMessageReceiveCommand = new DelegateCommand(_ => { serviceBusMessageReceiver.Stop(); },
            _ => !StartMessageReceiveCommand.CanExecute(default));
      }

      private void SetIdleReceiverStatus()
      {
         ReceiverStatusText = "Idle";
         ReceiverEnumStatus = ReceiverEnumStatus.Idle;
      }

      private void SetStoppedOnErrorReceiverStatus(Exception exception)
      {
         ReceiverStatusText = "Stopped because of error";
         ReceiverEnumStatus = ReceiverEnumStatus.StoppedOnError;
      }

      private void SetListeningReceiverStatus()
      {
         ReceiverStatusText = "Listening...";
         ReceiverEnumStatus = ReceiverEnumStatus.Listening;
      }

      private string FormatReceivedMessage(ReceivedMessage msg)
      {
         return $"{TimeUtils.GetShortTimestamp()} received message: \n{msg.Body}\n----------------------------------";
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
}

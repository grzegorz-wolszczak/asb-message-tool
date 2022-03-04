using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Main.Commands;
using Main.Models;
using Main.Utils;

namespace Main.ViewModels.Configs.Senders;

public interface ISenderConfigWindowDetacher
{
   void DetachFromPanel(SenderConfigViewModelWrapper item);
}

public class SenderConfigViewModel : INotifyPropertyChanged
{
   private readonly ISenderConfigWindowDetacher _senderConfigWindowDetacher;
   private readonly IMessageSender _messageSender;
   private SenderConfigModel _item;
   private bool _isEmbeddedInsideRightPanel = true;
   private string _lastSendStatus = "N/A";
   private string _messageToSendStringContent;
   private IInGuiThreadActionCaller _inGuiThreadActionCaller;

   public readonly SenderConfigViewModelWrapper ViewModelWrapper;

   public ICommand DetachFromPanelCommand { get; }
   public ICommand SendMessageCommand { get; }

   public SenderConfigViewModel(ISenderConfigWindowDetacher senderConfigWindowDetacher,
      IMessageSender messageSender,
      IInGuiThreadActionCaller inGuiThreadActionCaller)
   {
      _senderConfigWindowDetacher = senderConfigWindowDetacher;
      _messageSender = messageSender;

      ViewModelWrapper = new SenderConfigViewModelWrapper(this);

      DetachFromPanelCommand = new DelegateCommand(onExecuteMethod: _ => { _senderConfigWindowDetacher.DetachFromPanel(ViewModelWrapper); });
      _inGuiThreadActionCaller = inGuiThreadActionCaller;


      SendMessageCommand = new SendServiceMessageCommand(_messageSender,
         msgProviderFunc: () =>
         {
            return new MessageToSendData()
            {
               ConnectionString = Item.ServiceBusConnectionString,
               MsgBody = MessageToSendStringContent,
               TopicName = Item.OutputTopicName
            };
         },
         onErrorAction: (e) => { SetLastSendStatus(e.Message); },
         onSuccessAction: (statusMessage) => { SetLastSendStatus(statusMessage); },
         onUnexpectedExceptionAction: (exception) => { },
         _inGuiThreadActionCaller);
   }

   private void SetLastSendStatus(string msg)
   {
      var status = $"{TimeUtils.GetShortTimestamp()} {msg}";
      LastSendStatus = status;
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
         OnPropertyChanged();
      }
   }

   public string LastSendStatus
   {
      get => _lastSendStatus;
      set
      {
         if (value == _lastSendStatus) return;
         _lastSendStatus = value;
         OnPropertyChanged();
      }
   }

   public string MessageToSendStringContent
   {
      get => _messageToSendStringContent;
      set
      {
         if (value == _messageToSendStringContent) return;
         _messageToSendStringContent = value;
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

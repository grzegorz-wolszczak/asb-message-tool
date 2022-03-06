using System.ComponentModel;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
using Main.Application.Logging;
using Main.Commands;
using Main.Models;
using Main.Utils;

namespace Main.ViewModels.Configs.Senders
{
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
      //private string _messageToSendStringContent;
      private IInGuiThreadActionCaller _inGuiThreadActionCaller;
      private readonly IServiceBusHelperLogger _logger;
      private TextDocument _msgToSendDocument = new TextDocument("");

      public readonly SenderConfigViewModelWrapper ViewModelWrapper;

      public ICommand DetachFromPanelCommand { get; }
      public ICommand SendMessageCommand { get; }


      public SenderConfigViewModel(ISenderConfigWindowDetacher senderConfigWindowDetacher,
         IMessageSender messageSender,
         IInGuiThreadActionCaller inGuiThreadActionCaller,
         IServiceBusHelperLogger logger)
      {
         _senderConfigWindowDetacher = senderConfigWindowDetacher;
         _messageSender = messageSender;

         ViewModelWrapper = new SenderConfigViewModelWrapper(this);

         DetachFromPanelCommand = new DelegateCommand(onExecuteMethod: _ => { _senderConfigWindowDetacher.DetachFromPanel(ViewModelWrapper); });
         _inGuiThreadActionCaller = inGuiThreadActionCaller;
         _logger = logger;


         SendMessageCommand = new SendServiceMessageCommand(_messageSender,
            msgProviderFunc: () =>
            {
               // must use inGuiThread caller because otherwise referencing AvalongEdit.TextDocument would throw exception saying that
               // this object can be read only in thread that created it
               return inGuiThreadActionCaller.InvokeFunction(() => new MessageToSendData()
               {
                  ConnectionString = Item.ServiceBusConnectionString,
                  MsgBody = TextDocument.Text,
                  TopicName = Item.OutputTopicName
               });

            },
            onErrorAction: (e) => { SetLastSendStatus(e.Message); },
            onSuccessAction: (statusMessage) => { SetLastSendStatus(statusMessage); },
            onUnexpectedExceptionAction: (exception) =>
            {
               _logger.LogException("While sending message exception happened: ", exception);
               SetLastSendStatus("Error");
            },
            _inGuiThreadActionCaller);
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

      // public string MessageToSendStringContent
      // {
      //    get => TextDocument.Text;
      //    set
      //    {
      //       //if (value == _messageToSendStringContent) return;
      //       //_messageToSendStringContent = value;
      //       TextDocument.Text = value;
      //       OnPropertyChanged();
      //       OnPropertyChanged(nameof(TextDocument));
      //    }
      // }

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
}

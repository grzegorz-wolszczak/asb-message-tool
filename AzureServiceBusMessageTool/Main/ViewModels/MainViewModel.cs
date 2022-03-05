using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Main.Application.Logging;
using Main.Commands;

namespace Main.ViewModels
{
   public interface ILogContentAppender
   {
      public void AddContent(string msg);
   }

   public class MainViewModel : INotifyPropertyChanged,
      ILogContentAppender

   {
      private readonly IAboutWindowProxy _aboutWindow;
      private string _logContent;
      private string _checkServiceBusStatusMessage = "N/A";
      private string _statusBarMessage = "";
      private ServiceBusHelperLogger _logger;
      private bool _shouldScrollToEndOnLogContentChange;
      private bool _shouldWrapLogContent;
      private int _logTextBoxFontSize;


      public event PropertyChangedEventHandler? PropertyChanged;
      public ICommand ClearLogsCommand { get; }

      public MainViewModel(IAboutWindowProxy aboutWindow)
      {
         _aboutWindow = aboutWindow;
         _logger = new ServiceBusHelperLogger(this);

         ClearLogsCommand = new DelegateCommand(_ => { LogContent = ""; });
         ShowAboutWindowCommand = new DelegateCommand(_ =>
         {
            _aboutWindow.Show();
         });
      }

      public int LogTextBoxFontSize
      {
         get => _logTextBoxFontSize;
         set
         {
            if (value == _logTextBoxFontSize) return;
            _logTextBoxFontSize = value;
            OnPropertyChanged();
         }
      }

      public ICommand ShowAboutWindowCommand { get; }

      public bool ShouldScrollToEndOnLogContentChange
      {
         get => _shouldScrollToEndOnLogContentChange;
         set
         {
            if (value == _shouldScrollToEndOnLogContentChange) return;
            _shouldScrollToEndOnLogContentChange = value;
            OnPropertyChanged();
         }
      }

      public string ServiceBusStatusMessage
      {
         get => _checkServiceBusStatusMessage;
         set
         {
            if (value == _checkServiceBusStatusMessage) return;
            _checkServiceBusStatusMessage = value;
            OnPropertyChanged();
         }
      }

      public string StatusBarMessage
      {
         get => _statusBarMessage;
         set
         {
            if (value == _statusBarMessage) return;
            _statusBarMessage = value;
            OnPropertyChanged();
         }
      }

      public string LogContent
      {
         get => _logContent;
         set
         {
            if (value == _logContent) return;
            _logContent = value;
            OnPropertyChanged();
         }
      }

      public bool ShouldWordWrapLogContent
      {
         get => _shouldWrapLogContent;
         set
         {
            if (value == _shouldWrapLogContent) return;
            _shouldWrapLogContent = value;
            OnPropertyChanged();
         }
      }



      protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      public void AddContent(string msg)
      {
         LogContent = (LogContent + msg);
      }

      public IServiceBusHelperLogger GetLogger()
      {
         return _logger;
      }
   }

   public interface IAboutWindowProxy
   {
      void Show();
   }
}

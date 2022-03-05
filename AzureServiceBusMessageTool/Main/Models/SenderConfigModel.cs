using System.ComponentModel;
using System.Runtime.CompilerServices;
using Main.Application;

namespace Main.Models
{
   public sealed class SenderConfigModel : INotifyPropertyChanged
   {
      private string _configName;
      private string _serviceBusConnectionString;
      private string _outputTopicName;
      private int _msgBodyTextFontSize = AppDefaults.DefaultTextBoxFontSize; // todo: is value should be in configview model ? maybe move it to

      public string ConfigId { get; init; }

      public string ConfigName
      {
         get => _configName;
         set
         {
            if (value == _configName) return;
            _configName = value;
            OnPropertyChanged();
         }
      }

      public int MsgBodyTextBoxFontSize
      {
         get => _msgBodyTextFontSize;
         set
         {
            if (value == _msgBodyTextFontSize) return;
            _msgBodyTextFontSize = value;
            OnPropertyChanged();
         }
      }

      public string ServiceBusConnectionString
      {
         get => _serviceBusConnectionString;
         set
         {
            if (value == _serviceBusConnectionString) return;
            _serviceBusConnectionString = value;
            OnPropertyChanged();
         }
      }

      public string OutputTopicName
      {
         get => _outputTopicName;
         set
         {
            if (value == _outputTopicName) return;
            _outputTopicName = value;
            OnPropertyChanged();
         }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      private void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
   }
}

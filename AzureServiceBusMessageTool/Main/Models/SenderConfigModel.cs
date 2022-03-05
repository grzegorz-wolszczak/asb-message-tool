using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.Models;

public sealed class SenderConfigModel : INotifyPropertyChanged
{
   private string _configName;
   private string _serviceBusConnectionString;
   private string _outputTopicName;

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

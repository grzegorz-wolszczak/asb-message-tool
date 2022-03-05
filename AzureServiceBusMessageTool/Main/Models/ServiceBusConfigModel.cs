using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.Models
{
   public sealed class ServiceBusConfigModel : INotifyPropertyChanged
   {
      private string _configName;
      private string _connectionString;

      public string ConfigId { get; init; } // UUID
      public event PropertyChangedEventHandler PropertyChanged;

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

      public string ConnectionString
      {
         get => _connectionString;
         set
         {
            if (value == _connectionString) return;
            _connectionString = value;
            OnPropertyChanged();
         }
      }

      private void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
   }
}

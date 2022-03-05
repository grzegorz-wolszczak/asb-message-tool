using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.Models;

public sealed class ReceiverConfigModel : INotifyPropertyChanged
{
   private string _configName;
   private string _connectionString;
   private string _inputTopicName;
   private string _inputTopicSubscriptionName;
   private bool _isAttachedToDeadLetterSubqueue;
   private bool _shouldScrollTextBoxToEndOnNewMessageReceive;
   private bool _shouldWordWrapLogContent;

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

   public bool IsAttachedToDeadLetterSubqueue
   {
      get => _isAttachedToDeadLetterSubqueue;
      set
      {
         if (value == _isAttachedToDeadLetterSubqueue) return;
         _isAttachedToDeadLetterSubqueue = value;
         OnPropertyChanged();
      }
   }

   public bool ShouldWordWrapLogContent
   {
      get => _shouldWordWrapLogContent;
      set
      {
         if (value == _shouldWordWrapLogContent) return;
         _shouldWordWrapLogContent = value;
         OnPropertyChanged();
      }
   }

   public string ServiceBusConnectionString
   {
      get => _connectionString;
      set
      {
         if (value == _connectionString) return;
         _connectionString = value;
         OnPropertyChanged();
      }
   }

   public string InputTopicName
   {
      get => _inputTopicName;
      set
      {
         if (value == _inputTopicName) return;
         _inputTopicName = value;
         OnPropertyChanged();
      }
   }

   public string InputTopicSubscriptionName
   {
      get => _inputTopicSubscriptionName;
      set
      {
         if (value == _inputTopicSubscriptionName) return;
         _inputTopicSubscriptionName = value;
         OnPropertyChanged();
      }
   }

   public bool ShouldScrollTextBoxToEndOnNewMessageReceive
   {
      get => _shouldScrollTextBoxToEndOnNewMessageReceive;
      set
      {
         if (value == _shouldScrollTextBoxToEndOnNewMessageReceive) return;
         _shouldScrollTextBoxToEndOnNewMessageReceive = value;
         OnPropertyChanged();
      }
   }

   public string ConfigId { get; init; } // UUID
   public event PropertyChangedEventHandler PropertyChanged;

   private void OnPropertyChanged([CallerMemberName] string propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}

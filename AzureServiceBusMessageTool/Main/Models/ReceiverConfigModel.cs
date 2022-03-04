﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.Models;

public sealed class ReceiverConfigModel : INotifyPropertyChanged
{
   private string _configName;
   private string _connectionString;
   private string _inputTopicName;
   private string _inputTopicSubscriptionName;

   public string ConfigName
   {
      get => _configName;
      set
      {
         if (value == _configName) return;
         _configName = value;
         OnPropertyChanged(nameof(ConfigName));
      }
   }

   public string ServiceBusConnectionString
   {
      get => _connectionString;
      set
      {
         if (value == _connectionString) return;
         _connectionString = value;
         OnPropertyChanged(nameof(ConfigName));
      }
   }

   public string InputTopicName
   {
      get => _inputTopicName;
      set
      {
         if (value == _inputTopicName) return;
         _inputTopicName = value;
         OnPropertyChanged(nameof(ConfigName));
      }
   }

   public string InputTopicSubscriptionName
   {
      get => _inputTopicSubscriptionName;
      set
      {
         if (value == _inputTopicSubscriptionName) return;
         _inputTopicSubscriptionName = value;
         OnPropertyChanged(nameof(ConfigName));
      }
   }


   public string ConfigId { get; init; } // UUID
   public event PropertyChangedEventHandler PropertyChanged;

   private void OnPropertyChanged([CallerMemberName] string propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}

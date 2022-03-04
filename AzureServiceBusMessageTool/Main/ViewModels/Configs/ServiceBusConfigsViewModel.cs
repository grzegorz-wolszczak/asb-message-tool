using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Main.Commands;
using Main.Models;

namespace Main.ViewModels.Configs;

public class ServiceBusSelectedConfigsViewModel : INotifyPropertyChanged
{

   private ServiceBusConfigModel _currentSelectedItem;


   private bool _isServiceBusConfigTabSelected;
   public bool IsServiceBusConfigTabSelected
   {
      get => _isServiceBusConfigTabSelected;
      set
      {
         if (value == _isServiceBusConfigTabSelected) return;
         _isServiceBusConfigTabSelected = value;
         OnPropertyChanged();
      }
   }

   public void AddConfigs(List<ServiceBusConfigModel> settingsServiceBusConfigs)
   {
      settingsServiceBusConfigs.ForEach(e =>
      {
         ServiceBusConfigs.Add(e);
      });
   }


   public ServiceBusSelectedConfigsViewModel()
   {

      AddServiceBusConfigCommand = new DelegateCommand(_ =>
      {
         var newConfig = new ServiceBusConfigModel()
         {
            ConfigId = Guid.NewGuid().ToString(),
            ConfigName = "<config name not set>",
            ConnectionString = ""
         };
         _serviceBusConfigs.Add(newConfig);

         CurrentSelectedItem = newConfig;

      });

      DeleteServiceBusConfigCommand = new DelegateCommand(
         onExecuteMethod: _ =>
         {
            // todo: ask if user wants to delete object
            _serviceBusConfigs.Remove(CurrentSelectedItem);
            CurrentSelectedItem = null;

         },
         onCanExecuteMethod: _ => CurrentSelectedItem != null);

   }

   private readonly IList<ServiceBusConfigModel> _serviceBusConfigs = new ObservableCollection<ServiceBusConfigModel>();

   public ServiceBusConfigModel CurrentSelectedItem
   {
      get => _currentSelectedItem;
      set
      {
         if (value == _currentSelectedItem) return;
         _currentSelectedItem = value;

         OnPropertyChanged();
      }
   }

   public bool CanShowSelectedItemInsideTabContent()
   {
      return CurrentSelectedItem != null;
   }

   public ICommand AddServiceBusConfigCommand { get; }

   public ICommand DeleteServiceBusConfigCommand { get; }


   public IList<ServiceBusConfigModel> ServiceBusConfigs => _serviceBusConfigs;

   public event PropertyChangedEventHandler PropertyChanged;

   protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}
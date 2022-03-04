using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Main.Commands;
using Main.ConfigsGuiMetadata;
using Main.Models;

namespace Main.ViewModels.Configs.Receivers;

public class ReceiversSelectedConfigViewModel : INotifyPropertyChanged
{
   private ReceiverConfigViewModel _currentSelectedItem;
   private bool _isReceiverConfigTabSelected;

   public bool IsReceiverConfigTabSelected
   {
      get => _isReceiverConfigTabSelected;
      set
      {
         if (value == _isReceiverConfigTabSelected) return;
         _isReceiverConfigTabSelected = value;
         OnPropertyChanged();
      }
   }

   public void AddConfigs(List<ReceiverConfigModel> settingsReceiversConfig)
   {
      settingsReceiversConfig.ForEach(AddNewConfig);
   }

   public ReceiversSelectedConfigViewModel(ServiceBusMessageReceiverFactory serviceBusMessageReceiverFactory)
   {
      _serviceBusMessageReceiverFactory = serviceBusMessageReceiverFactory;
      _receiverConfigElementsGuiMetadataManager = new ReceiverConfigElementsGuiMetadataManager();

      AddReceiverConfigCommand = new DelegateCommand(_ =>
      {
         var newConfig = new ReceiverConfigModel()
         {
            ConfigId = Guid.NewGuid().ToString(),
            ConfigName = "<config name not set>",
         };

         AddNewConfig(newConfig);
      });

      DeleteReceiverConfigCommand = new DelegateCommand(_ =>
         {
            _receiverConfigElementsGuiMetadataManager.Delete(CurrentSelectedConfigModelItem.ViewModelWrapper);
            ReceiversConfigs.Remove(CurrentSelectedConfigModelItem);
         },
         _ => CurrentSelectedConfigModelItem != null);
   }

   private void AddNewConfig(ReceiverConfigModel newConfig)
   {
      var viewModel = new ReceiverConfigViewModel(_receiverConfigElementsGuiMetadataManager, _serviceBusMessageReceiverFactory)
      {
         Item = newConfig
      };
      _receiverConfigElementsGuiMetadataManager.Add(viewModel.ViewModelWrapper);
      ReceiversConfigs.Add(viewModel);
   }

   public event PropertyChangedEventHandler PropertyChanged;

   protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }

   private readonly IList<ReceiverConfigViewModel> _receiversConfigs = new ObservableCollection<ReceiverConfigViewModel>();
   private ReceiverConfigElementsGuiMetadataManager _receiverConfigElementsGuiMetadataManager;
   public IList<ReceiverConfigViewModel> ReceiversConfigs => _receiversConfigs;

   public ReceiverConfigViewModel CurrentSelectedConfigModelItem
   {
      get => _currentSelectedItem;
      set
      {
         if (value == _currentSelectedItem) return;
         _currentSelectedItem = value;
         IsEmbeddedSenderConfigUserControlForEditingEnabled = _currentSelectedItem != null;

         OnPropertyChanged();
      }
   }

   private bool _isEmbeddedSenderConfigUserControlForEditingEnabled;
   private ServiceBusMessageReceiverFactory _serviceBusMessageReceiverFactory;

   public bool IsEmbeddedSenderConfigUserControlForEditingEnabled
   {
      get => _isEmbeddedSenderConfigUserControlForEditingEnabled;
      set
      {
         if (value == _isEmbeddedSenderConfigUserControlForEditingEnabled) return;
         _isEmbeddedSenderConfigUserControlForEditingEnabled = value;

         OnPropertyChanged();
      }
   }


   public ICommand AddReceiverConfigCommand { get; }

   public ICommand DeleteReceiverConfigCommand { get; }
}

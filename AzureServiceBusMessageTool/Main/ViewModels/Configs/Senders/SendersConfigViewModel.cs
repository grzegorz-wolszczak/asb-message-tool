using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Main.Application;
using Main.Application.Logging;
using Main.Commands;
using Main.ConfigsGuiMetadata;
using Main.Models;

namespace Main.ViewModels.Configs.Senders
{
   public class SendersSelectedConfigViewModel : INotifyPropertyChanged
   {
      private SenderConfigViewModel _currentSelectedItem;
      private bool _isSenderConfigConfigTabSelected;
      public ICommand AddSenderConfigCommand { get; }
      public ICommand DeleteSenderConfigCommand { get; }

      public bool IsSenderConfigTabSelected
      {
         get => _isSenderConfigConfigTabSelected;
         set
         {
            if (value == _isSenderConfigConfigTabSelected) return;
            _isSenderConfigConfigTabSelected = value;
            OnPropertyChanged();
         }
      }

      public void AddConfigs(List<SenderConfigModel> settingsSendersConfig)
      {
         settingsSendersConfig.ForEach(AddNewConfig);
      }

      public SendersSelectedConfigViewModel(SenderConfigElementsGuiMetadataManager senderConfigElementsGuiMetadataManager,
         InGuiThreadActionCaller inGuiThreadActionCaller,
         MessageSenderFactory messageSenderFactory,
         IServiceBusHelperLogger logger)
      {
         _inGuiThreadActionCaller = inGuiThreadActionCaller;
         _messageSenderFactory = messageSenderFactory;
         _logger = logger;
         _senderConfigElementsGuiMetadataManager = senderConfigElementsGuiMetadataManager;

         AddSenderConfigCommand = new DelegateCommand(_ =>
         {
            var newConfig = new SenderConfigModel()
            {
               ConfigId = Guid.NewGuid().ToString(),
               ConfigName = "<config name not set>",
            };
            AddNewConfig(newConfig);
         });

         DeleteSenderConfigCommand = new DelegateCommand(_ =>
            {
               _senderConfigElementsGuiMetadataManager.Delete(CurrentSelectedConfigModelItem.ViewModelWrapper);
               SendersConfigs.Remove(CurrentSelectedConfigModelItem);
            },
            _ => CurrentSelectedConfigModelItem != null);
      }

      private void AddNewConfig(SenderConfigModel newConfig)
      {
         var viewModel = new SenderConfigViewModel(
            _senderConfigElementsGuiMetadataManager,
            _messageSenderFactory.Create(),
            _inGuiThreadActionCaller,
            _logger)
         {
            Item = newConfig
         };
         _senderConfigElementsGuiMetadataManager.Add(viewModel.ViewModelWrapper);
         SendersConfigs.Add(viewModel);
      }

      private SenderConfigElementsGuiMetadataManager _senderConfigElementsGuiMetadataManager;

      public event PropertyChangedEventHandler PropertyChanged;


      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      private readonly IList<SenderConfigViewModel> _sendersConfigs = new ObservableCollection<SenderConfigViewModel>();
      public IList<SenderConfigViewModel> SendersConfigs => _sendersConfigs;

      public SenderConfigViewModel CurrentSelectedConfigModelItem
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
      private InGuiThreadActionCaller _inGuiThreadActionCaller;
      private readonly MessageSenderFactory _messageSenderFactory;
      private readonly IServiceBusHelperLogger _logger;

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
   }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Main.ViewModels.Configs;
using Main.ViewModels.Configs.Receivers;
using Main.ViewModels.Configs.Senders;

namespace Main.ViewModels;


public class LeftPanelControlViewModel : INotifyPropertyChanged
{
   private readonly ServiceBusSelectedConfigsViewModel _serviceBusConfigsViewModel;
   private readonly SendersSelectedConfigViewModel _sendersViewModel;
   private readonly ReceiversSelectedConfigViewModel _receiversViewModel;

   private bool _leftPanelServiceBusConfigTabIsSelected;
   private bool _leftPanelSenderConfigTabIsSelected;
   private bool _leftPanelReceiversConfigTabIsSelected;



   public event PropertyChangedEventHandler PropertyChanged;


   public LeftPanelControlViewModel(
      ServiceBusSelectedConfigsViewModel serviceBusConfigsViewModel,
      SendersSelectedConfigViewModel sendersViewModel,
      ReceiversSelectedConfigViewModel receiversViewModel)
   {
      _serviceBusConfigsViewModel = serviceBusConfigsViewModel;
      _sendersViewModel = sendersViewModel;
      _receiversViewModel = receiversViewModel;
   }


   protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }


   public bool LeftPanelServiceBusConfigTabIsSelected
   {
      get => _leftPanelServiceBusConfigTabIsSelected;
      set
      {
         if (value == _leftPanelServiceBusConfigTabIsSelected) return;
         _leftPanelServiceBusConfigTabIsSelected = value;
         if (_leftPanelServiceBusConfigTabIsSelected)
         {
            _serviceBusConfigsViewModel.IsServiceBusConfigTabSelected = true;
         }
         OnPropertyChanged();
      }
   }

   public bool LeftPanelSenderConfigTabIsSelected
   {
      get => _leftPanelSenderConfigTabIsSelected;
      set
      {
         if (value == _leftPanelSenderConfigTabIsSelected) return;
         _leftPanelSenderConfigTabIsSelected = value;
         if (_leftPanelSenderConfigTabIsSelected)
         {
            _sendersViewModel.IsSenderConfigTabSelected = true;
         }

         OnPropertyChanged();
      }
   }

   public bool LeftPanelReceiverConfigTabIsSelected
   {
      get => _leftPanelReceiversConfigTabIsSelected;
      set
      {
         if (value == _leftPanelReceiversConfigTabIsSelected) return;
         _leftPanelReceiversConfigTabIsSelected = value;
         if (_leftPanelReceiversConfigTabIsSelected)
         {
            _receiversViewModel.IsReceiverConfigTabSelected = true;
         }
         OnPropertyChanged();
      }
   }
}
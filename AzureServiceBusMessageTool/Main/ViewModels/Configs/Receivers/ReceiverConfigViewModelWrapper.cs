using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.ViewModels.Configs.Receivers;

// this class is needed because when Receiver config window is opened and its data context is set
// current DataContext view model must have 'CurrentSelectedConfigModelItem' member/propoerty
public class ReceiverConfigViewModelWrapper : INotifyPropertyChanged
{
   private ReceiverConfigViewModel _receiverConfigViewModel;

   public ReceiverConfigViewModelWrapper(ReceiverConfigViewModel ReceiverConfigViewModel)
   {
      _receiverConfigViewModel = ReceiverConfigViewModel;
   }

   public event PropertyChangedEventHandler PropertyChanged;

   public ReceiverConfigViewModel CurrentSelectedConfigModelItem
   {
      get => _receiverConfigViewModel;
      set
      {
         if (value == _receiverConfigViewModel) return;
         _receiverConfigViewModel = value;
         OnPropertyChanged();
      }
   }

   protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Main.Commands;
using Main.ViewModels.Configs.Senders.MessagePropertyWindow;

namespace Main.ViewModels.Configs.Senders;

public sealed class SbMessageFieldsViewModel : INotifyPropertyChanged
{
   private  IList<SBMessageApplicationProperty> _messageApplicationProperties;
   private  SbMessageStandardFields _sbMessageStandardFields;

   private SBMessageApplicationProperty _selectedItem;

   public SbMessageFieldsViewModel(
      IList<SBMessageApplicationProperty> messageApplicationProperties,
      SbMessageStandardFields sbMessageStandardFields)
   {
      _messageApplicationProperties = messageApplicationProperties;
      _sbMessageStandardFields = sbMessageStandardFields;
      AddMessagePropertyCommand = new DelegateCommand(_ =>
      {
         _messageApplicationProperties.Add(new SBMessageApplicationProperty()
         {
            PropertyName = "",
            PropertyValue = ""
         });
      });
      DeleteMessagePropertyCommand = new DelegateCommand(_ =>
      {
         if (SelectedItem != null)
         {
            _messageApplicationProperties.Remove(SelectedItem);
         }
      },_=> SelectedItem != null);
   }

   public event PropertyChangedEventHandler PropertyChanged;

   public ICommand AddMessagePropertyCommand { get; }
   public ICommand DeleteMessagePropertyCommand { get; }

   public SbMessageStandardFields MessageFields
   {
      get => _sbMessageStandardFields;
      set
      {
         if (value == _sbMessageStandardFields) return;
         _sbMessageStandardFields = value;
         OnPropertyChanged();
      }

   }

   public SBMessageApplicationProperty SelectedItem
   {
      get => _selectedItem;
      set
      {
         if (value == _selectedItem) return;
         _selectedItem = value;
         OnPropertyChanged();
      }
   }

   public IList<SBMessageApplicationProperty> ApplicationProperties
   {
      get => _messageApplicationProperties;
   }

   private void OnPropertyChanged([CallerMemberName] string propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}

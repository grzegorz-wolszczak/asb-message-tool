using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ASBMessageTool.Application;

namespace ASBMessageTool.Model;

public sealed class SbMessageFieldsViewModel : INotifyPropertyChanged, IServiceBusMessageApplicationPropertiesHolder
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
         _messageApplicationProperties.Add(new SBMessageApplicationProperty
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

   public void RemoveEmptyProperties()
   {
       var itemsToRemove = _messageApplicationProperties.Where(e => string.IsNullOrWhiteSpace(e.PropertyName)).ToList();
       foreach (var itemToRemove in itemsToRemove)
       {
           _messageApplicationProperties.Remove(itemToRemove);
       }
   }

   public IList<string> GetDuplicatedApplicationProperties()
   {
       return _messageApplicationProperties.GroupBy(x => x.PropertyName)
           .Where(g => g.Count() > 1)
           .Select(x => x.Key)
           .ToList();
   }
}

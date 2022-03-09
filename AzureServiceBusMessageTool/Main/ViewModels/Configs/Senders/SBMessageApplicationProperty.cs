using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Main.ViewModels.Configs.Senders;

public class SBMessageApplicationProperty : INotifyPropertyChanged
{
   private string _name;
   private string _value;

   public string PropertyName
   {
      get { return _name; }
      set
      {
         _name = value;
         OnPropertyChanged();
      }
   }

   public string PropertyValue
   {
      get { return _value; }
      set
      {
         _value = value;
         OnPropertyChanged();
      }
   }

   public event PropertyChangedEventHandler PropertyChanged;

   protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }
}

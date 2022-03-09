using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Main.Converters
{
   public class BoolToTextWrappingValueConverter : IValueConverter
   {
      public object Convert(object value,
         Type targetType,
         object parameter,
         CultureInfo culture)
      {
         if (targetType != typeof(TextWrapping))
         {
            throw new InvalidCastException($"The target must be a {typeof(TextWrapping)} but is '{targetType.FullName}'");
         }

         if ((bool) value)
         {
            return TextWrapping.Wrap;
         }

         return TextWrapping.NoWrap;
      }

      public object ConvertBack(object value,
         Type targetType,
         object parameter,
         CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }

}


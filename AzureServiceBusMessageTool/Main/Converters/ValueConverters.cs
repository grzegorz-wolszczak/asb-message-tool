using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Main.Converters;

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

public class EnumToBooleanConverter : IValueConverter
{
    // Convert enum [value] to boolean, true if matches [param]
    public object Convert(object value, Type targetType, object param, CultureInfo culture)
    {
        return value.Equals(param);
    }

    // Convert boolean to enum, returning [param] if true
    public object ConvertBack(object value, Type targetType, object param, CultureInfo culture)
    {
        return (bool)value ? param : Binding.DoNothing;
    }
}

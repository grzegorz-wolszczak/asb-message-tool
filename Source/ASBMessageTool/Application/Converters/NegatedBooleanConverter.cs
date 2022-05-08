using System;
using System.Globalization;
using System.Windows.Data;

namespace ASBMessageTool.Application.Converters;

public class NegatedBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            throw new InvalidCastException($"The target must be a {typeof(bool)} but is '{targetType.FullName}'");
        }

        if (value is not bool)
        {
            throw new InvalidCastException($"The value must be a {typeof(bool)} but is '{value.GetType().FullName}'");
        }

        var bValue = (bool)value;

        return !bValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // not implemented on purpose
        throw new NotImplementedException();
    }
}

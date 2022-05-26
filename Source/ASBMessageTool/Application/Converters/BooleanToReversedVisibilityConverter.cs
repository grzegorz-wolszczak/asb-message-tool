using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ASBMessageTool.Application.Converters;

public sealed class BooleanToReversedVisibilityConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool bValue = false;
        if (value is bool)
        {
            bValue = (bool)value;
        }

        return (bValue) ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // not implemented on purpose
        throw new NotImplementedException();
    }
}



public sealed class BooleanToHiddenVisibilityConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool bValue = false;
        if (value is bool)
        {
            bValue = (bool)value;
        }

        return (bValue) ? Visibility.Visible: Visibility.Hidden ;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // not implemented on purpose
        throw new NotImplementedException();
    }
}

#nullable enable
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Infrastructure.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
        {
            throw new ArgumentException("Аргумент 'value' должен иметь тип bool");
        }

        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    { 
        throw new NotImplementedException(); 
    }
}

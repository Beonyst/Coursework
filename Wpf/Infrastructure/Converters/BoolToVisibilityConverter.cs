#nullable enable
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Infrastructure.Converters;

// Конвертер, преобразующий значение типа bool в видимость (Visibility).
public class BoolToVisibilityConverter : IValueConverter
{
    // Преобразует значение типа bool в видимость (Visibility).
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Проверка типа значения, должно быть типа bool
        if (value is not bool boolValue)
        {
            throw new ArgumentException("Аргумент 'value' должен иметь тип bool");
        }

        // Возвращает Visibility.Visible, если значение true, иначе Visibility.Collapsed
        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    // Метод преобразования в обратную сторону не реализован
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    { 
        throw new NotImplementedException(); 
    }
}

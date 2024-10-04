using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace H2_Gruppe_project.Converters
{
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString(culture);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && decimal.TryParse(stringValue, out decimal result))
            {
                return result;
            }
            return 0m; // default decimal value if conversion fails
        }
    }
}

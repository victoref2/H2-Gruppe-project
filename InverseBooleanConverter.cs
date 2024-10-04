using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace H2_Gruppe_project.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean)
                return !boolean;
            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean)
                return !boolean;
            return AvaloniaProperty.UnsetValue;
        }
    }
}


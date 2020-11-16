using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Avalonia.Data.Converters;

namespace AvaloniaControlTest.Demo
{
    public class Zero2NanConverter : IValueConverter
    {
        public static Zero2NanConverter Instance { get; } = new Zero2NanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                if ((double)value > 0)
                    return value;
            }

            return double.NaN;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double && double.IsNaN((double) value))
                return 0d;
            else
                return value;
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SvgViewer
{
    public class IntToSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Size((double)value, (double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

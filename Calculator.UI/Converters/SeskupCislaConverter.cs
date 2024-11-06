using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Calculator.UI.Converters
{
    internal class SeskupCislaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Regex.Replace((string)value, @"(\d+)", "\u200B$1\u200B");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Regex.Replace((string)value, @"\u200B", "");
        }
    }
}

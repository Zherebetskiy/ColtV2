using System.Globalization;

namespace Colt.UI.Desktop.Converters
{
    public class DoubleToFormattedStringConverter : IValueConverter
    {
        private readonly CultureInfo _culture = new CultureInfo("uk-UA"); // Ukrainian culture

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double number)
            {
                return number.ToString("#,##0.00", _culture); // Format with space and comma
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                strValue = strValue.Replace(" ", "").Replace(",", "."); // Normalize input for parsing
                if (double.TryParse(strValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                {
                    return number;
                }
            }
            return 0.0; // Default fallback
        }
    }
}

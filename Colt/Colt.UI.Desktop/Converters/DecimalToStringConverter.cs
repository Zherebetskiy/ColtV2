using System.Globalization;

namespace Colt.UI.Desktop.Converters
{
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                var q = decimalValue.ToString("#,##0.00", culture);
                return q; // Format as "1 234,56" in uk-UA
            }
            return value; // Default value
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                // Replace spaces and use invariant parsing
                if (decimal.TryParse(stringValue.Replace(" ", "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                {
                    return result;
                }
            }
            return 0m; // Default fallback
        }
    }
}

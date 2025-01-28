using Colt.Domain.Enums;
using System.Globalization;

namespace Colt.UI.Desktop.Converters
{
    public class MeasurmentToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MeasurementType measurementType)
            {
                return measurementType switch
                {
                    MeasurementType.Weight => "за кілограм",
                    MeasurementType.Quantity => "за одиницю",
                    _ => value
                };
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string orderStatus)
            {
                return orderStatus switch
                {
                    "за кілограм" => MeasurementType.Weight,
                    "за одиницю" => MeasurementType.Quantity,
                    _ => value
                };
            }

            return value;
        }
    }
}

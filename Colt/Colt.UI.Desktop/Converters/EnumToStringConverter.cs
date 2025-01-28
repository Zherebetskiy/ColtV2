using Colt.Domain.Enums;
using System.Globalization;

namespace Colt.UI.Desktop.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderStatus orderStatus)
            {
                return orderStatus switch
                {
                    OrderStatus.Created => "Нове",
                    OrderStatus.Calculated => "Поважено",
                    OrderStatus.Delivered => "Доставлено",
                    _ => value
                };
            }

            if (value is MeasurementType measurementType)
            {
                return measurementType switch
                {
                    MeasurementType.Weight => "Вага",
                    MeasurementType.Quantity => "Кількість",
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
                    "Нове" => OrderStatus.Created,
                    "Поважено" => OrderStatus.Calculated,
                    "Доставлено" => OrderStatus.Delivered,
                    "Вага" => MeasurementType.Weight,
                    "Кількість" => MeasurementType.Quantity,
                    _ => value
                };
            }

            return value;
        }
    }
}

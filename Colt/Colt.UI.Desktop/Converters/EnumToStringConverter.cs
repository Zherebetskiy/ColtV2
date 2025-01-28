using Colt.Domain.Enums;
using System.Globalization;

namespace Colt.UI.Desktop.Converters
{
    internal class EnumToStringConverter : IValueConverter
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
                    _ => value
                };
            }

            return value;
        }
    }
}

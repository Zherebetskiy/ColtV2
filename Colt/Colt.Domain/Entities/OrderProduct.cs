using Colt.Domain.Common;
using Colt.Domain.Enums;

namespace Colt.Domain.Entities
{ 
    public class OrderProduct : BaseEntity<int>
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public MeasurementType? ProductType { get; set; }

        public double? OrderedWeight { get; set; }

        public double? ActualWeight { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}

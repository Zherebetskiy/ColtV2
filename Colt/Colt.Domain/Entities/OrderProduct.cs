using Colt.Domain.Common;

namespace Colt.Domain.Entities
{ 
    public class OrderProduct : BaseEntity<int>
    {
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int CustomerProductId { get; set; }
        public CustomerProduct? Product { get; set; }

        public double? OrderedWeight { get; set; }

        public double? ActualWeight { get; set; }

        public decimal? TotalPrice { get; set; }
    }
}

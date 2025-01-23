using Colt.Domain.Common;

namespace Colt.Domain.Entities
{
    public class CustomerProduct : BaseEntity<int>
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal Price { get; set; }
    }
}

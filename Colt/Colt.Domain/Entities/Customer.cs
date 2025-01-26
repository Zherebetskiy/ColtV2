using Colt.Domain.Common;

namespace Colt.Domain.Entities
{
    public class Customer : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Notes { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<CustomerProduct> Products { get; set; } = new List<CustomerProduct>();

        public ICollection<Order> Orders { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}

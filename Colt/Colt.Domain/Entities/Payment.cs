using Colt.Domain.Common;

namespace Colt.Domain.Entities
{
    public class Payment : BaseEntity<int>
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}

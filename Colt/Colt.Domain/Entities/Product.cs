using Colt.Domain.Common;

namespace Colt.Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

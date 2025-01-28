using Colt.Domain.Common;
using Colt.Domain.Enums;

namespace Colt.Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public MeasurementType MeasurementType { get; set; }
        public string Description { get; set; }
    }
}

using Colt.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Colt.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Colt.Infrastructure.Persistance.Configurations
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.Property(x => x.OrderId)
                .IsRequired(true);

            builder.Property(x => x.TotalPrice)
                .IsRequired(false);

            builder.Property(x => x.OrderedWeight)
               .IsRequired(false);

            builder.Property(x => x.ActualWeight)
               .IsRequired(false);

            builder.Property(x => x.ProductName)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.ProductType)
                .HasDefaultValue(MeasurementType.Weight)
                .HasConversion<EnumToStringConverter<MeasurementType>>()
                .IsRequired(false);

            builder.Property(x => x.ProductPrice)
                .IsRequired(false);
        }
    }
}

using Colt.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Colt.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Colt.Infrastructure.Persistance.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired(true);

            builder.Property(x => x.Description)
                .HasMaxLength(700)
                .IsRequired(false);

            builder.Property(x => x.MeasurementType)
                .HasDefaultValue(MeasurementType.Weight)
                .HasConversion<EnumToStringConverter<MeasurementType>>()
                .IsRequired(true);

            builder.Property(x => x.CreatedDate)
                .IsRequired(true);

            builder.Property(x => x.UpdatedDate)
                .IsRequired(false);

            builder.ToTable("Products");
        }
    }
}

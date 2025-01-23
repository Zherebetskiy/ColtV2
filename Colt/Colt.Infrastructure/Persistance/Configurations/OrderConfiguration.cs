using Colt.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Colt.Domain.Enums;

namespace Colt.Infrastructure.Persistance.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Date)
                .IsRequired(true);

            builder.Property(x => x.Delivery)
                .IsRequired(true);

            builder.Property(x => x.TotalPrice)
                .IsRequired(false);

            builder.Property(x => x.TotalWeight)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .HasConversion<EnumToStringConverter<OrderStatus>>()
                .IsRequired(true);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId);

            builder.HasMany(x => x.Products)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Colt.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Colt.Infrastructure.Persistance.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Configure primary key
            builder.HasKey(x => x.Id);

            // Configure properties
            builder.Property(x => x.Date)
                .IsRequired(true);

            builder.Property(x => x.Amount)
                .IsRequired(true);

            // Configure inherited properties
            builder.Property(x => x.CreatedDate)
                .IsRequired(true);

            builder.Property(x => x.UpdatedDate)
                .IsRequired(false);

            // Configure relationships with cascade delete
            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure table name
            builder.ToTable("Payments");
        }
    }
}

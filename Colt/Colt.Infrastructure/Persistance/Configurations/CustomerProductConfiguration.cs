using Colt.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Colt.Infrastructure.Persistance.Configurations
{
    public class CustomerProductConfiguration : IEntityTypeConfiguration<CustomerProduct>
    {
        public void Configure(EntityTypeBuilder<CustomerProduct> builder)
        {
            // Configure primary key
            builder.HasKey(x => x.Id);

            // Configure properties
            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired(true);

            // Configure relationships
            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure table name
            builder.ToTable("CustomerProducts");

        }
    }
}
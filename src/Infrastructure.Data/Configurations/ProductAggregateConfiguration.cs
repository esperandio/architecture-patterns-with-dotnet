using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain;

namespace Infrastructure.Data.Configurations;

public class ProductAggregateConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Sku);

        builder
            .OwnsMany(
                x => x.Batches,
                y => 
                {
                    y.ToTable("Batches");
                    y.HasKey("Reference");
                    y
                        .OwnsMany(
                            x => x.Allocations,
                            y => 
                            {
                                y.ToTable("OrderLines");
                                y.HasKey(x => new { x.OrderId, x.Quantity,  x.Sku });
                                y.WithOwner().HasForeignKey("Reference");
                            }
                        )
                        .WithOwner().HasForeignKey("Sku");
                }
            );
        
        builder
            .Property(x => x.Version)
            .IsConcurrencyToken();

        builder.Ignore(x => x.DomainEvents);
    }
}
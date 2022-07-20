using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain;

namespace Infrastructure.Persistence.EntityConfigurations;

internal class OrderLineEntityTypeConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.ToTable("order_lines");

        builder.HasKey((x) => x.Id);

        builder
            .Property<int>("Id")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("id");

        builder
            .Property<string>("_orderId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("order_id")
            .IsRequired();

        builder
            .Property<int>("_quantity")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("quantity")
            .IsRequired();

        builder
            .Property<string>("_sku")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("sku")
            .IsRequired();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain;

namespace Infrastructure.Persistence.EntityConfigurations;

internal class BatchEntityTypeConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("batches");

        builder.HasKey((x) => x.Id);

        builder
            .Property<int>("Id")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("id");

        builder
            .Property<string>("_reference")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("reference")
            .IsRequired();

        builder
            .Property<string>("_sku")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("sku")
            .IsRequired();

        builder
            .Property<int>("_purchasedQuantity")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("purchased_quantity")
            .IsRequired();

        builder
            .Property<DateTime?>("_eta")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("eta");

        builder
            .HasMany(x => x.Allocations)
            .WithOne()
            .HasForeignKey("batch_id");
    }
}
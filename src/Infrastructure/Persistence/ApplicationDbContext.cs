using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.EntityConfigurations;
using Domain;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Batch> Batches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderLineEntityTypeConfiguration());       
        modelBuilder.ApplyConfiguration(new BatchEntityTypeConfiguration());
    }
}
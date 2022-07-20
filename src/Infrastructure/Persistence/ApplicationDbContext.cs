using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.EntityConfigurations;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderLineEntityTypeConfiguration());       
        modelBuilder.ApplyConfiguration(new BatchEntityTypeConfiguration());
    }
}
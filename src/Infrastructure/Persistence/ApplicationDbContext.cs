using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Models;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Batch> Batches { get; set; }
    public DbSet<Allocation> Allocations { get; set; }
}
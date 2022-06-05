using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Models;

namespace Infrastructure.Persistence;

class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
}
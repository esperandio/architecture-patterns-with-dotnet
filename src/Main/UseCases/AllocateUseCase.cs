using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Main.UseCases;

public class AllocateUseCase
{
    private ApplicationDbContext _dbContext;

    public AllocateUseCase(ApplicationDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public string Perform(AllocateData allocateData)
    {
        var batches = _dbContext.Batches.Include(x => x.Allocations).ToList();

        var orderLine = new OrderLine(
            allocateData.OrderId,
            allocateData.Sku,
            allocateData.Qty
        );

        var reference = AllocationService.Allocate(orderLine, batches);

        _dbContext.SaveChanges();

        return reference;
    }
}

public class AllocateData
{
    public string OrderId { get; set; }
    public string Sku { get; set; }
    public int Qty { get; set; }
}
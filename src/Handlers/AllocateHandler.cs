using Domain;

namespace Handlers;

public class AllocateHandler
{
    private readonly IUnitOfWork uow;

    public AllocateHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Handle(AllocationRequiredEvent allocationRequiredEvent)
    {
        var product = await uow.Products.Get(allocationRequiredEvent.Sku);

        if (product == null)
        {
            throw new InvalidSkuException(allocationRequiredEvent.Sku);
        }

        var batchReference = product.Allocate(
            allocationRequiredEvent.OrderId, 
            allocationRequiredEvent.Sku, 
            allocationRequiredEvent.Qty
        );

        await uow.Commit();

        return batchReference;
    }
}

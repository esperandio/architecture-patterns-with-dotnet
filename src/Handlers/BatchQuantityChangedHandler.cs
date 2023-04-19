using Domain;

namespace Handlers;

public class BatchQuantityChangedHandler
{
    private readonly IUnitOfWork uow;

    public BatchQuantityChangedHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task Handle(BatchQuantityChangedEvent batchQuantityChangedEvent)
    {
        var product = await uow.Products.GetByBatchReference(batchQuantityChangedEvent.Reference);

        if (product == null)
        {
            return;
        }

        product.ChangeBatchQuantity(batchQuantityChangedEvent.Reference, batchQuantityChangedEvent.Quantity);

        await uow.Commit();
    }
}
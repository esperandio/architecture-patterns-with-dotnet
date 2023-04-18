using Domain;

namespace Handlers;

public class AddBatchHandler
{
    private readonly IUnitOfWork uow;

    public AddBatchHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Handle(BatchCreatedEvent batchCreatedEvent)
    {
        var product = await uow.Products.Get(batchCreatedEvent.Sku);

        if (product == null)
        {
            throw new InvalidSkuException(batchCreatedEvent.Sku);
        }

        product.AddBatch(
            batchCreatedEvent.Reference, 
            batchCreatedEvent.Sku, 
            batchCreatedEvent.PurchasedQuantity, 
            batchCreatedEvent.Eta
        );

        await uow.Commit();

        return batchCreatedEvent.Reference;
    }
}

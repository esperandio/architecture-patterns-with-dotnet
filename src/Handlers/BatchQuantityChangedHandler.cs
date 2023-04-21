using Domain;

namespace Handlers;

public class BatchQuantityChangedHandler
{
    private readonly IUnitOfWork uow;

    public BatchQuantityChangedHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task Handle(ChangeBatchQuantityCommand ChangeBatchQuantityCommand)
    {
        var product = await uow.Products.GetByBatchReference(ChangeBatchQuantityCommand.Reference);

        if (product == null)
        {
            return;
        }

        product.ChangeBatchQuantity(ChangeBatchQuantityCommand.Reference, ChangeBatchQuantityCommand.Quantity);

        await uow.Commit();
    }
}
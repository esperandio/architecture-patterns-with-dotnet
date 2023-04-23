using Domain;
using Handlers.Abstractions;

namespace Handlers;

class BatchQuantityChangedHandler
{
    private readonly IUnitOfWork uow;

    public BatchQuantityChangedHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task Handle(ChangeBatchQuantityCommand changeBatchQuantityCommand)
    {
        var product = await uow.Products.GetByBatchReference(changeBatchQuantityCommand.Reference);

        if (product == null)
        {
            return;
        }

        product.ChangeBatchQuantity(changeBatchQuantityCommand.Reference, changeBatchQuantityCommand.Quantity);

        await uow.Commit();
    }
}
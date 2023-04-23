using Domain;
using Handlers.Abstractions;
using Handlers.Exceptions;

namespace Handlers;

class AddBatchHandler
{
    private readonly IUnitOfWork uow;

    public AddBatchHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Handle(CreateBatchCommand createBatchCommand)
    {
        var product = await uow.Products.Get(createBatchCommand.Sku);

        if (product == null)
        {
            throw new InvalidSkuException(createBatchCommand.Sku);
        }

        product.AddBatch(
            createBatchCommand.Reference, 
            createBatchCommand.Sku, 
            createBatchCommand.PurchasedQuantity, 
            createBatchCommand.Eta
        );

        await uow.Commit();

        return createBatchCommand.Reference;
    }
}

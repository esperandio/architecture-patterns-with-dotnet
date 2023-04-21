using Domain;

namespace Handlers;

public class AddBatchHandler
{
    private readonly IUnitOfWork uow;

    public AddBatchHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Handle(CreateBatchCommand CreateBatchCommand)
    {
        var product = await uow.Products.Get(CreateBatchCommand.Sku);

        if (product == null)
        {
            throw new InvalidSkuException(CreateBatchCommand.Sku);
        }

        product.AddBatch(
            CreateBatchCommand.Reference, 
            CreateBatchCommand.Sku, 
            CreateBatchCommand.PurchasedQuantity, 
            CreateBatchCommand.Eta
        );

        await uow.Commit();

        return CreateBatchCommand.Reference;
    }
}

using Domain;

namespace Handlers;

public class AllocateHandler
{
    private readonly IUnitOfWork uow;

    public AllocateHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Handle(AllocateCommand AllocateCommand)
    {
        var product = await uow.Products.Get(AllocateCommand.Sku);

        if (product == null)
        {
            throw new InvalidSkuException(AllocateCommand.Sku);
        }

        var batchReference = product.Allocate(
            AllocateCommand.OrderId, 
            AllocateCommand.Sku, 
            AllocateCommand.Qty
        );

        await uow.Commit();

        return batchReference;
    }
}

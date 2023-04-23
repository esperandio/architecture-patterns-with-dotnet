using Domain;

namespace Handlers;

class AllocateHandler
{
    private readonly IUnitOfWork uow;

    public AllocateHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Handle(AllocateCommand allocateCommand)
    {
        var product = await uow.Products.Get(allocateCommand.Sku);

        if (product == null)
        {
            throw new InvalidSkuException(allocateCommand.Sku);
        }

        var batchReference = product.Allocate(
            allocateCommand.OrderId, 
            allocateCommand.Sku, 
            allocateCommand.Qty
        );

        await uow.Commit();

        return batchReference;
    }
}

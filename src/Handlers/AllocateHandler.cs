namespace Handlers;

public class AllocateHandler
{
    private readonly IUnitOfWork uow;

    public AllocateHandler(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Handle(AllocateData allocateData)
    {
        var product = await uow.Products.Get(allocateData.Sku);

        if (product == null)
        {
            throw new InvalidSkuException(allocateData.Sku);
        }

        var batchReference = product.Allocate(
            allocateData.OrderId, 
            allocateData.Sku, 
            allocateData.Qty
        );

        await uow.Commit();

        return batchReference;
    }

    public async Task<string> Handle(string reference, AllocateData allocateData)
    {
        var product = await uow.Products.Get(allocateData.Sku);

        if (product == null)
        {
            throw new InvalidSkuException(allocateData.Sku);
        }

        var batchReference = product.AllocateToSpecificBatch(
            reference,
            allocateData.OrderId, 
            allocateData.Sku, 
            allocateData.Qty
        );

        await uow.Commit();

        return batchReference;
    }
}

public class AllocateData
{
    public string OrderId { get; set; } = "";
    public string Sku { get; set; }  = "";
    public int Qty { get; set; }
}
using Domain;

namespace UseCases;

public class DeallocateUseCase
{
    private readonly IUnitOfWork uow;

    public DeallocateUseCase(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Perform(DeallocateData deallocateData)
    {
        var product = await uow.Products.Get(deallocateData.Sku);

        if (product == null)
        {
            throw new InvalidSkuException(deallocateData.Sku);
        }

        var batchReference = product.Deallocate(
            deallocateData.OrderId, 
            deallocateData.Sku, 
            deallocateData.Qty
        );

        await uow.Commit();

        return batchReference;
    }
}

public class DeallocateData
{
    public string OrderId { get; set; } = "";
    public string Sku { get; set; }  = "";
    public int Qty { get; set; }
}
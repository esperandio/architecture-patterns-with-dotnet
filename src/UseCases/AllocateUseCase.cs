using Domain;

namespace UseCases;

public class AllocateUseCase
{
    private readonly IUnitOfWork uow;

    public AllocateUseCase(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Perform(AllocateData allocateData)
    {
        var product = await uow.Products.Get(allocateData.Sku);

        if (product == null)
        {
            throw new Exception("Invalid SKU");
        }

        var batchReference = product.Allocate(
            new OrderLine(allocateData.OrderId, allocateData.Sku, allocateData.Qty)
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
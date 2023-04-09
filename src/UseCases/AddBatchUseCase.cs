using Domain;

namespace UseCases;

public class AddBatchUseCase
{
    private readonly IUnitOfWork uow;

    public AddBatchUseCase(IUnitOfWork unitOfWork)
    {
        uow = unitOfWork;
    }

    public async Task<string> Perform(AddBatchData addBatchData)
    {
        var product = await uow.Products.Get(addBatchData.Sku);

        if (product == null)
        {
            throw new Exception("Invalid SKU");
        }

        product.AddBatch(
            addBatchData.Reference, 
            addBatchData.Sku, 
            addBatchData.PurchasedQuantity, 
            addBatchData.Eta
        );

        await uow.Commit();

        return addBatchData.Reference;
    }
}

public record AddBatchData
{
    public string Reference { get; set; } = "";
    public string Sku { get; set; }  = "";
    public int PurchasedQuantity { get; set; }
    public DateTime? Eta { get; set; }
}
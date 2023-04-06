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
        await uow.Batches.Add(
            new Batch(
                addBatchData.Reference, 
                addBatchData.Sku, 
                addBatchData.PurchasedQuantity, 
                addBatchData.Eta
            )
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
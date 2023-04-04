using Domain;

namespace UseCases;

public class AddBatchUseCase
{
    private readonly IBatchRepository _repository;

    public AddBatchUseCase(IBatchRepository batchRepository)
    {
        _repository = batchRepository;
    }

    public async Task<string> Perform(AddBatchData addBatchData)
    {
        await _repository.Add(
            new Batch(
                addBatchData.Reference, 
                addBatchData.Sku, 
                addBatchData.PurchasedQuantity, 
                addBatchData.Eta
            )
        );

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
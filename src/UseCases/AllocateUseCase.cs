using Domain;

namespace UseCases;

public class AllocateUseCase
{
    private readonly IBatchRepository _batchRepository;

    public AllocateUseCase(IBatchRepository batchRepository)
    {
        _batchRepository = batchRepository;
    }

    public async Task<string> Perform(AllocateData allocateData)
    {
        var batches = await _batchRepository.FindBySkuAsync(allocateData.Sku);

        var orderLine = new OrderLine(allocateData.OrderId, allocateData.Sku, allocateData.Qty);

        var batchReference = AllocationService.Allocate(orderLine, batches);

        return batchReference;
    }
}

public class AllocateData
{
    public string OrderId { get; set; } = "";
    public string Sku { get; set; }  = "";
    public int Qty { get; set; }
}
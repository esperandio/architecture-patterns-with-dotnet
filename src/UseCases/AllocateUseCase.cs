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
        var batches = await uow.Batches.FindBySkuAsync(allocateData.Sku);

        var orderLine = new OrderLine(allocateData.OrderId, allocateData.Sku, allocateData.Qty);

        var batchReference = AllocationService.Allocate(orderLine, batches);

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
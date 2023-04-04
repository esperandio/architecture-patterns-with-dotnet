namespace UseCases.Tests;

public class AllocateUseCaseTest
{
    [Fact]
    public async void TestAllocateReturnsReference()
    {
        var repository = new FakeBatchRepository();

        var addBatchService = new AddBatchUseCase(repository);

        await addBatchService.Perform(new AddBatchData()
        {
            Reference = "slow-batch",
            Sku = "MINIMALIST-SPOON",
            PurchasedQuantity = 50,
            Eta =  new DateTime().AddDays(2)
        });
        
        await addBatchService.Perform(new AddBatchData()
        {
            Reference = "speedy-batch",
            Sku = "MINIMALIST-SPOON",
            PurchasedQuantity = 50
        });

        var batchReference = await new AllocateUseCase(repository).Perform(new AllocateData()
        {
            OrderId = "order001",
            Qty = 10,
            Sku = "MINIMALIST-SPOON"
        });

        Assert.Equal("speedy-batch", batchReference);
    }
}
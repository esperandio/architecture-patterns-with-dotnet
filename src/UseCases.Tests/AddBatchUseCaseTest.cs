namespace UseCases.Tests;

public class AddBatchUseCaseTest
{
    [Fact]
    public async void TestAddBatch()
    {
        var uow = new FakeUnitOfWork();
        
        var reference = await new AddBatchUseCase(uow).Perform(new AddBatchData()
        {
            Reference = "batch001",
            Sku = "MINIMALIST-SPOON",
            PurchasedQuantity = 10,
            Eta = DateTime.Now
        });

        Assert.Equal("batch001", reference);
        Assert.NotNull(await uow.Batches.Get("batch001"));
    }
}
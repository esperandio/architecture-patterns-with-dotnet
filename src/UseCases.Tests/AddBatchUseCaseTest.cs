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

        var product = await uow.Products.Get("MINIMALIST-SPOON");

        Assert.Equal("batch001", reference);
        Assert.NotNull(product?.Batches.First(x => x.Reference == "batch001"));
    }
}
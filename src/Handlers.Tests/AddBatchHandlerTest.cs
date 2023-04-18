namespace Handlers.Tests;

public class AddBatchHandlerTest
{
    [Fact]
    public async void TestAddBatch()
    {
        var uow = new FakeUnitOfWork();
        
        var reference = await new AddBatchHandler(uow).Perform(new AddBatchData()
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

    [Fact]
    public async void TestCannotAddBatchIfSKUDoesNotExist()
    {
        var uow = new FakeUnitOfWork();

        await Assert.ThrowsAsync<InvalidSkuException>(async () => {
            await new AddBatchHandler(uow).Perform(new AddBatchData()
            {
                Reference = "batch001",
                Sku = "INVALID-SKU",
                PurchasedQuantity = 10,
                Eta = DateTime.Now
            });
        });
    }
}
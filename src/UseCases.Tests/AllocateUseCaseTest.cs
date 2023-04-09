namespace UseCases.Tests;

public class AllocateUseCaseTest
{
    [Fact]
    public async void TestAllocateReturnsReference()
    {
        var uow = new FakeUnitOfWork();

        var addBatchService = new AddBatchUseCase(uow);

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

        var batchReference = await new AllocateUseCase(uow).Perform(new AllocateData()
        {
            OrderId = "order001",
            Qty = 10,
            Sku = "MINIMALIST-SPOON"
        });

        Assert.Equal("speedy-batch", batchReference);
    }

    [Fact]
    public async void TestAvailableQuantityIsReducedWhenOrderLineIsAllocated()
    {
        var uow = new FakeUnitOfWork();

        var addBatchService = new AddBatchUseCase(uow);

        await addBatchService.Perform(new AddBatchData()
        {
            Reference = "batch-001",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 20
        });

        await new AllocateUseCase(uow).Perform(new AllocateData()
        {
            OrderId = "order-001",
            Sku = "SMALL-TABLE",
            Qty = 2
        });

        var product = await uow.Products.Get("SMALL-TABLE");

        Assert.Equal(18, product?.BatchAvailableQuantity("batch-001"));
    }
}
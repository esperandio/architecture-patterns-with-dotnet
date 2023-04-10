namespace UseCases.Tests;

public class DeallocateUseCaseTest
{
    private readonly FakeUnitOfWork uow;

    public DeallocateUseCaseTest()
    {
        uow = new FakeUnitOfWork();
    }

    [Fact]
    public async void TestAvailableQuantityIsIncreasedWhenOrderLineIsDeallocated()
    {
        var addBatchService = new AddBatchUseCase(uow);
        var allocateService = new AllocateUseCase(uow);
        var deallocateService = new DeallocateUseCase(uow);

        await addBatchService.Perform(new AddBatchData()
        {
            Reference = "batch001",
            Sku = "MINIMALIST-SPOON",
            PurchasedQuantity = 20
        });

        await allocateService.Perform(new AllocateData()
        {
            OrderId = "order001",
            Qty = 2,
            Sku = "MINIMALIST-SPOON"
        });

        var productBeforeDeallocate = await uow.Products.Get("MINIMALIST-SPOON");

        Assert.Equal(18, productBeforeDeallocate?.BatchAvailableQuantity("batch001"));

        await deallocateService.Perform(new DeallocateData()
        {
            OrderId = "order001",
            Qty = 2,
            Sku = "MINIMALIST-SPOON"
        });

        var productAfterDeallocate = await uow.Products.Get("MINIMALIST-SPOON");

        Assert.Equal(20, productBeforeDeallocate?.BatchAvailableQuantity("batch001"));
    }
}
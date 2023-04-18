using Domain;

namespace Handlers.Tests;

public class DeallocateHandlerTest
{
    private readonly FakeUnitOfWork uow;

    public DeallocateHandlerTest()
    {
        uow = new FakeUnitOfWork();
    }

    [Fact]
    public async void TestAvailableQuantityIsIncreasedWhenOrderLineIsDeallocated()
    {
        var addBatchService = new AddBatchHandler(uow);
        var allocateService = new AllocateHandler(uow);
        var deallocateService = new DeallocateHandler(uow);

        await addBatchService.Handle(new AddBatchData()
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

    [Fact]
    public async void TestCannotDeallocateIfSKUDoesNotExist()
    {
        await Assert.ThrowsAsync<InvalidSkuException>(async () => {
            await new DeallocateHandler(uow).Perform(new DeallocateData()
            {
                OrderId = "order001",
                Qty = 10,
                Sku = "INVALID-SKU"
            });
        });
    }

    [Fact]
    public async void TestCannotDeallocateUnallocatedOrderLine()
    {
        await Assert.ThrowsAsync<UnallocatedOrderLineException>(async () => {
            await new DeallocateHandler(uow).Perform(new DeallocateData()
            {
                OrderId = "order001",
                Qty = 10,
                Sku = "SMALL-TABLE"
            });
        });
    }
}
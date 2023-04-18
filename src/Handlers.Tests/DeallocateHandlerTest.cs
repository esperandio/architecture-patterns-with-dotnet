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

        await addBatchService.Handle(
            new BatchCreatedEvent("batch001", "MINIMALIST-SPOON", 20)
        );

        await allocateService.Handle(
            new AllocationRequiredEvent("order001", "MINIMALIST-SPOON", 2)
        );

        var productBeforeDeallocate = await uow.Products.Get("MINIMALIST-SPOON");

        Assert.Equal(18, productBeforeDeallocate?.BatchAvailableQuantity("batch001"));

        await deallocateService.Handle(new DeallocateData()
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
            await new DeallocateHandler(uow).Handle(new DeallocateData()
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
            await new DeallocateHandler(uow).Handle(new DeallocateData()
            {
                OrderId = "order001",
                Qty = 10,
                Sku = "SMALL-TABLE"
            });
        });
    }
}
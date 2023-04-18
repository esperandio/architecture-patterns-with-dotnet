using Domain;

namespace Handlers.Tests;

public class AllocateHandlerTest
{
    private readonly FakeUnitOfWork uow;

    public AllocateHandlerTest()
    {
        uow = new FakeUnitOfWork();
    }

    [Fact]
    public async void TestAllocateReturnsReference()
    {
        var addBatchService = new AddBatchHandler(uow);
        var allocateService = new AllocateHandler(uow);

        await addBatchService.Handle(
            new BatchCreatedEvent("slow-batch", "MINIMALIST-SPOON", 50, new DateTime().AddDays(2))
        );
        
        await addBatchService.Handle(
            new BatchCreatedEvent("speedy-batch", "MINIMALIST-SPOON", 50)
        );

        var batchReference = await allocateService.Handle(new AllocateData()
        {
            OrderId = "order001",
            Qty = 10,
            Sku = "MINIMALIST-SPOON"
        });

        Assert.Equal("speedy-batch", batchReference);
    }

    [Fact]
    public async void TestCannotAllocateIfSKUDoesNotExist()
    {
        var uow = new FakeUnitOfWork();

        await Assert.ThrowsAsync<InvalidSkuException>(async () => {
            await new AllocateHandler(uow).Handle(new AllocateData()
            {
                OrderId = "order001",
                Qty = 10,
                Sku = "INVALID-SKU"
            });
        });
    }

    [Fact]
    public async void TestAvailableQuantityIsReducedWhenOrderLineIsAllocated()
    {
        var addBatchService = new AddBatchHandler(uow);
        var allocateService = new AllocateHandler(uow);

        await addBatchService.Handle(
            new BatchCreatedEvent("batch-001", "SMALL-TABLE", 20)
        );

        await allocateService.Handle(new AllocateData()
        {
            OrderId = "order-001",
            Sku = "SMALL-TABLE",
            Qty = 2
        });

        var product = await uow.Products.Get("SMALL-TABLE");

        Assert.Equal(18, product?.BatchAvailableQuantity("batch-001"));
    }

    [Fact]
    public async void TestCannotAllocateIfAvailableSmallerThanRequired()
    {
        var addBatchService = new AddBatchHandler(uow);
        var allocateService = new AllocateHandler(uow);

        await addBatchService.Handle(
            new BatchCreatedEvent("batch-001", "SMALL-TABLE", 1)
        );

        await Assert.ThrowsAsync<RequiresQuantityGreaterThanAvailableException>(async () => {
            await allocateService.Handle(
                "batch-001", 
                new AllocateData()
                {
                    OrderId = "order-001",
                    Sku = "SMALL-TABLE",
                    Qty = 2
                }
            );
        });
    }

    [Fact]
    public async void TestCannotAllocateTheSameOrderLineTwice()
    {
        var addBatchService = new AddBatchHandler(uow);
        var allocateService = new AllocateHandler(uow);

        await addBatchService.Handle(
            new BatchCreatedEvent("batch-001", "SMALL-TABLE", 10)
        );

        await allocateService.Handle(
            "batch-001",
            new AllocateData()
            {
                OrderId = "order-001",
                Sku = "SMALL-TABLE",
                Qty = 2
            }
        );

        await Assert.ThrowsAsync<DuplicateOrderLineException>(async () => {
            await allocateService.Handle(
                "batch-001",
                new AllocateData()
                {
                    OrderId = "order-001",
                    Sku = "SMALL-TABLE",
                    Qty = 2
                }
            );
        });
    }

    [Fact]
    public async void TestPrefersCurrentStockBatchesToShipment()
    {
        var addBatchService = new AddBatchHandler(uow);
        var allocateService = new AllocateHandler(uow);

        await addBatchService.Handle(
            new BatchCreatedEvent("shipment-batch", "SMALL-TABLE", 100, DateTime.Now.AddDays(1))
        );

        await addBatchService.Handle(
            new BatchCreatedEvent("in-stock-batch", "SMALL-TABLE", 100)
        );

        await allocateService.Handle(new AllocateData()
        {
            OrderId = "order-001",
            Sku = "SMALL-TABLE",
            Qty = 10
        });

        var product = await uow.Products.Get("SMALL-TABLE");

        Assert.Equal(100, product?.BatchAvailableQuantity("shipment-batch"));
        Assert.Equal(90, product?.BatchAvailableQuantity("in-stock-batch"));
    }

    [Fact]
    public async void TestPrefersEarlierBatches()
    {
        var addBatchService = new AddBatchHandler(uow);
        var allocateService = new AllocateHandler(uow);

        await addBatchService.Handle(
            new BatchCreatedEvent("normal-batch", "SMALL-TABLE", 100, DateTime.Today.AddDays(1))
        );

        await addBatchService.Handle(
            new BatchCreatedEvent("slow-batch", "SMALL-TABLE", 100, DateTime.Today.AddDays(2))
        );

        await addBatchService.Handle(
            new BatchCreatedEvent("speedy-batch", "SMALL-TABLE", 100, DateTime.Today)
        );

        await allocateService.Handle(new AllocateData()
        {
            OrderId = "order-001",
            Sku = "SMALL-TABLE",
            Qty = 10
        });

        var product = await uow.Products.Get("SMALL-TABLE");

        Assert.Equal(100, product?.BatchAvailableQuantity("normal-batch"));
        Assert.Equal(100, product?.BatchAvailableQuantity("slow-batch"));
        Assert.Equal(90, product?.BatchAvailableQuantity("speedy-batch"));
    }
}
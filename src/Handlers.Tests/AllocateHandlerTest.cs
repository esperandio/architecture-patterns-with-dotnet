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

        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "slow-batch",
            Sku = "MINIMALIST-SPOON",
            PurchasedQuantity = 50,
            Eta =  new DateTime().AddDays(2)
        });
        
        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "speedy-batch",
            Sku = "MINIMALIST-SPOON",
            PurchasedQuantity = 50
        });

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

        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "batch-001",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 20
        });

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

        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "batch-001",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 1
        });

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

        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "batch-001",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 10
        });

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

        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "shipment-batch",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 100,
            Eta = DateTime.Now.AddDays(1)
        });

        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "in-stock-batch",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 100
        });

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

        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "normal-batch",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 100,
            Eta = DateTime.Today.AddDays(1)
        });

        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "slow-batch",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 100,
            Eta = DateTime.Today.AddDays(2)
        });

        await addBatchService.Handle(new AddBatchData()
        {
            Reference = "speedy-batch",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 100,
            Eta = DateTime.Today
        });

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
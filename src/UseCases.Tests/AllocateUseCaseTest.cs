using Domain;

namespace UseCases.Tests;

public class AllocateUseCaseTest
{
    private readonly FakeUnitOfWork uow;

    public AllocateUseCaseTest()
    {
        uow = new FakeUnitOfWork();
    }

    [Fact]
    public async void TestAllocateReturnsReference()
    {
        var addBatchService = new AddBatchUseCase(uow);
        var allocateService = new AllocateUseCase(uow);

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

        var batchReference = await allocateService.Perform(new AllocateData()
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
        var addBatchService = new AddBatchUseCase(uow);
        var allocateService = new AllocateUseCase(uow);

        await addBatchService.Perform(new AddBatchData()
        {
            Reference = "batch-001",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 20
        });

        await allocateService.Perform(new AllocateData()
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
        var addBatchService = new AddBatchUseCase(uow);
        var allocateService = new AllocateUseCase(uow);

        await addBatchService.Perform(new AddBatchData()
        {
            Reference = "batch-001",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 1
        });

        await Assert.ThrowsAsync<RequiresQuantityGreaterThanAvailableException>(async () => {
            await allocateService.Perform(new AllocateData()
            {
                OrderId = "order-001",
                Sku = "SMALL-TABLE",
                Qty = 2,
                BatchReference = "batch-001"
            });
        });
    }

    [Fact]
    public async void TestCannotAllocateTheSameOrderLineTwice()
    {
        var addBatchService = new AddBatchUseCase(uow);
        var allocateService = new AllocateUseCase(uow);

        await addBatchService.Perform(new AddBatchData()
        {
            Reference = "batch-001",
            Sku = "SMALL-TABLE",
            PurchasedQuantity = 10
        });

        await allocateService.Perform(new AllocateData()
        {
            OrderId = "order-001",
            Sku = "SMALL-TABLE",
            Qty = 2,
            BatchReference = "batch-001"
        });

        await Assert.ThrowsAsync<DuplicateOrderLineException>(async () => {
            await allocateService.Perform(new AllocateData()
            {
                OrderId = "order-001",
                Sku = "SMALL-TABLE",
                Qty = 2,
                BatchReference = "batch-001"
            });
        });
    }
}
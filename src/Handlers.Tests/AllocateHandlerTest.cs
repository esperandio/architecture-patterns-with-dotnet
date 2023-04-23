using Domain;

namespace Handlers.Tests;

public class AllocateHandlerTest
{
    [Fact]
    public async void TestAllocateReturnsReference()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();
        var messageBroker = new FakeMessageBroker();

        var messageBus = new MessageBus(uow, mailService, messageBroker);

        await messageBus.Handle(
            new CreateBatchCommand("slow-batch", "MINIMALIST-SPOON", 50, new DateTime().AddDays(2))
        );
        
        await messageBus.Handle(
            new CreateBatchCommand("speedy-batch", "MINIMALIST-SPOON", 50)
        );

        await messageBus.Handle(
            new AllocateCommand("order001", "MINIMALIST-SPOON", 10)
        );

        Assert.Equal("speedy-batch", messageBus.Results.Last());
    }

    [Fact]
    public async void TestCannotAllocateIfSKUDoesNotExist()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();
        var messageBroker = new FakeMessageBroker();

        var messageBus = new MessageBus(uow, mailService, messageBroker);

        await Assert.ThrowsAsync<InvalidSkuException>(async () => {
            await messageBus.Handle(
                new AllocateCommand("order001", "INVALID-SKU", 10)
            );
        });
    }

    [Fact]
    public async void TestAvailableQuantityIsReducedWhenOrderLineIsAllocated()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();
        var messageBroker = new FakeMessageBroker();

        var messageBus = new MessageBus(uow, mailService, messageBroker);

        await messageBus.Handle(
            new CreateBatchCommand("batch-001", "SMALL-TABLE", 20)
        );

        await messageBus.Handle(
            new AllocateCommand("order-001", "SMALL-TABLE", 2)
        );

        var product = await uow.Products.Get("SMALL-TABLE");

        Assert.Equal(18, product?.BatchAvailableQuantity("batch-001"));
    }

    [Fact]
    public async void TestPrefersCurrentStockBatchesToShipment()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();
        var messageBroker = new FakeMessageBroker();

        var messageBus = new MessageBus(uow, mailService, messageBroker);

        await messageBus.Handle(
            new CreateBatchCommand("shipment-batch", "SMALL-TABLE", 100, DateTime.Now.AddDays(1))
        );

        await messageBus.Handle(
            new CreateBatchCommand("in-stock-batch", "SMALL-TABLE", 100)
        );

        await messageBus.Handle(
            new AllocateCommand("order-001", "SMALL-TABLE", 10)
        );

        var product = await uow.Products.Get("SMALL-TABLE");

        Assert.Equal(100, product?.BatchAvailableQuantity("shipment-batch"));
        Assert.Equal(90, product?.BatchAvailableQuantity("in-stock-batch"));
    }

    [Fact]
    public async void TestPrefersEarlierBatches()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();
        var messageBroker = new FakeMessageBroker();

        var messageBus = new MessageBus(uow, mailService, messageBroker);

        await messageBus.Handle(
            new CreateBatchCommand("normal-batch", "SMALL-TABLE", 100, DateTime.Today.AddDays(1))
        );

        await messageBus.Handle(
            new CreateBatchCommand("slow-batch", "SMALL-TABLE", 100, DateTime.Today.AddDays(2))
        );

        await messageBus.Handle(
            new CreateBatchCommand("speedy-batch", "SMALL-TABLE", 100, DateTime.Today)
        );

        await messageBus.Handle(
            new AllocateCommand("order-001", "SMALL-TABLE", 10)
        );

        var product = await uow.Products.Get("SMALL-TABLE");

        Assert.Equal(100, product?.BatchAvailableQuantity("normal-batch"));
        Assert.Equal(100, product?.BatchAvailableQuantity("slow-batch"));
        Assert.Equal(90, product?.BatchAvailableQuantity("speedy-batch"));
    }
}
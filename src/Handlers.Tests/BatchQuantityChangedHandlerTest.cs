using Domain;

namespace Handlers.Tests;

public class BatchQuantityChangedHandlerTest
{
    [Fact]
    public async void TestChangesAvailableQuantity()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();

        var messageBus = new MessageBus(uow, mailService);

        await messageBus.Handle(new CreateBatchCommand("batch001", "SMALL-TABLE", 100));

        var product = await uow.Products.Get("SMALL-TABLE");

        if (product == null)
        {
            return;
        }

        Assert.Equal(100, product.BatchAvailableQuantity("batch001"));

        await messageBus.Handle(new ChangeBatchQuantityCommand("batch001", 50));

        Assert.Equal(50, product.BatchAvailableQuantity("batch001"));
    }

    [Fact]
    public async void TestReallocatesIfNecessary()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();

        var messageBus = new MessageBus(uow, mailService);

        await messageBus.Handle(new CreateBatchCommand("batch001", "SMALL-TABLE", 100));
        await messageBus.Handle(new CreateBatchCommand("batch002", "SMALL-TABLE", 100));

        await messageBus.Handle(new AllocateCommand("order001", "SMALL-TABLE", 50));

        var product = await uow.Products.Get("SMALL-TABLE");

        if (product == null)
        {
            return;
        }

        Assert.Equal(50, product.BatchAvailableQuantity("batch001"));
        Assert.Equal(100, product.BatchAvailableQuantity("batch002"));

        await messageBus.Handle(new ChangeBatchQuantityCommand("batch001", 30));

        Assert.Equal(30, product.BatchAvailableQuantity("batch001"));
        Assert.Equal(50, product.BatchAvailableQuantity("batch002"));
    }
}
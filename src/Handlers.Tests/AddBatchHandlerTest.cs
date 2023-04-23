using Domain;
using Handlers.Tests.Doubles;

namespace Handlers.Tests;

public class AddBatchHandlerTest
{
    [Fact]
    public async void TestAddBatch()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();
        var messageBroker = new FakeMessageBroker();

        var messageBus = new MessageBus(uow, mailService, messageBroker);

        await messageBus.Handle(new CreateBatchCommand("batch001", "MINIMALIST-SPOON", 10, DateTime.Now));
        
        var product = await uow.Products.Get("MINIMALIST-SPOON");

        Assert.Equal("batch001", messageBus.Results.First());
        Assert.NotNull(product?.Batches.First(x => x.Reference == "batch001"));
    }

    [Fact]
    public void TestCannotAddBatchIfSKUDoesNotExist()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();
        var messageBroker = new FakeMessageBroker();

        var messageBus = new MessageBus(uow, mailService, messageBroker);

        Assert.ThrowsAsync<InvalidSkuException>(async () => {
            await messageBus.Handle(new CreateBatchCommand("batch001", "INVALID-SKU", 10, DateTime.Now));
        });
    }
}
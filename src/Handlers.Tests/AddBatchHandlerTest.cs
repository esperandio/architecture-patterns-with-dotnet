using Domain;

namespace Handlers.Tests;

public class AddBatchHandlerTest
{
    [Fact]
    public async void TestAddBatch()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();

        var messageBus = new MessageBus(uow, mailService);

        await messageBus.Handle(new BatchCreatedEvent("batch001", "MINIMALIST-SPOON", 10, DateTime.Now));
        
        var product = await uow.Products.Get("MINIMALIST-SPOON");

        Assert.Equal("batch001", messageBus.Results.First());
        Assert.NotNull(product?.Batches.First(x => x.Reference == "batch001"));
    }

    [Fact]
    public void TestCannotAddBatchIfSKUDoesNotExist()
    {
        var uow = new FakeUnitOfWork();
        var mailService = new FakeMailService();

        var messageBus = new MessageBus(uow, mailService);

        Assert.ThrowsAsync<InvalidSkuException>(async () => {
            await messageBus.Handle(new BatchCreatedEvent("batch001", "INVALID-SKU", 10, DateTime.Now));
        });
    }
}
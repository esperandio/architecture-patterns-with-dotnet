using Domain;

namespace UseCases.Tests;

public class AddBatchUseCaseTest
{
    [Fact]
    public async void TestAddBatch()
    {
        var repository = new FakeBatchRepository(new List<Batch>());
        
        var reference = await new AddBatchUseCase(repository).Perform(new AddBatchData()
        {
            Reference = "batch001",
            Sku = "MINIMALIST-SPOON",
            PurchasedQuantity = 10,
            Eta = DateTime.Now
        });

        Assert.Equal("batch001", reference);
        Assert.NotNull(await repository.Get("batch001"));
    }
}
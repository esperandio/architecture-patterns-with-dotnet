using Domain;

namespace UseCases.Tests;

public class AllocateUseCaseTest
{
    [Fact]
    public async void TestAllocateReturnsReference()
    {
        var batches = new List<Batch>()
        {
            new Batch("slow-batch", "MINIMALIST-SPOON", 50, new DateTime().AddDays(2)),
            new Batch("speedy-batch", "MINIMALIST-SPOON", 50)
        };

        var repository = new FakeBatchRepository(batches);
        var useCase = new AllocateUseCase(repository);

        var batchReference = await useCase.Perform(new AllocateData()
        {
            OrderId = "order001",
            Qty = 10,
            Sku = "MINIMALIST-SPOON"
        });

        Assert.Equal("speedy-batch", batchReference);
    }
}
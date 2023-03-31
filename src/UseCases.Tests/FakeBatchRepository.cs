using Domain;

namespace UseCases.Tests;

class FakeBatchRepository : IBatchRepository
{
    private List<Batch> _batches;

    public FakeBatchRepository(List<Batch> batches)
    {
        _batches = batches;
    }

    public Task<IEnumerable<Batch>> FindBySkuAsync(string sku)
    {
        return Task.Run(() => _batches.Where(x => x.Sku == sku));
    }
}
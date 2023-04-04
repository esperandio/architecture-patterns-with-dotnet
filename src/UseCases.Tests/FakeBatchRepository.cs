using Domain;

namespace UseCases.Tests;

class FakeBatchRepository : IBatchRepository
{
    private List<Batch> _batches;

    public FakeBatchRepository()
    : this(new List<Batch>())
    {
    }

    public FakeBatchRepository(List<Batch> batches)
    {
        _batches = batches;
    }

    public Task<IEnumerable<Batch>> FindBySkuAsync(string sku)
    {
        return Task.Run(() => _batches.Where(x => x.Sku == sku));
    }

    public Task Add(Batch batch)
    {
        return Task.Run(() => _batches.Add(batch));
    }

    public Task<Batch?> Get(string reference)
    {
        return Task.Run(() => _batches.FirstOrDefault(x => x.Reference == reference));
    }
}
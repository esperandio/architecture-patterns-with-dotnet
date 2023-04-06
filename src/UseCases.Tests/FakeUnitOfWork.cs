using Domain;

namespace UseCases.Tests;

class FakeUnitOfWork : IUnitOfWork
{
    public IBatchRepository Batches { get; }

    public FakeUnitOfWork()
    {
        Batches = new FakeBatchRepository();
    }

    public Task<int> Commit()
    {
        return Task.Run(() => 1);
    }
}
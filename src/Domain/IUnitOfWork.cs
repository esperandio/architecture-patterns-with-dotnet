namespace Domain;

public interface IUnitOfWork
{
    IBatchRepository Batches { get; }
    Task<int> Commit();
}
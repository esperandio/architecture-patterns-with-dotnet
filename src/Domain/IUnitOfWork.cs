namespace Domain;

public interface IUnitOfWork
{
    IBatchRepository Batches { get; }
}
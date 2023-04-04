namespace Domain;

public interface IBatchRepository
{
    Task<IEnumerable<Batch>> FindBySkuAsync(string sku);
    Task Add(Batch batch);
    Task<Batch?> Get(string reference);
}
namespace Domain;

public interface IBatchRepository
{
    Task<IEnumerable<Batch>> FindBySkuAsync(string sku);
}
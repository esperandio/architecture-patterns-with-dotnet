namespace Domain;

public class AllocationService
{
    public static string Allocate(OrderLine orderLine, IEnumerable<Batch> batches)
    {
        var batch = batches
            .Where(x => x.CanAllocate(orderLine))
            .OrderBy(x => x.Eta)
            .FirstOrDefault();

        if (batch == null)
        {
            throw new OutOfStockException();
        }
        
        batch.Allocate(orderLine);

        return batch.Reference;
    }
}
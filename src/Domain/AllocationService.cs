namespace Domain;

public class AllocationService
{
    public static string Allocate(OrderLine orderLine, IEnumerable<Batch> batches)
    {
        var batch = batches
            .Where(x => x.CanAllocate(orderLine))
            .OrderBy(x => x.Eta)
            .First();
        
        batch.Allocate(orderLine);

        return batch.Reference;
    }
}
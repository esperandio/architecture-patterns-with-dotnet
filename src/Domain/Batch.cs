namespace Domain;

public class Batch
{
    private string _reference;
    private string _sku;
    private int _purchasedQuantity;
    private List<OrderLine> _allocations;
    private DateTime? _eta;
    public string Reference {get => _reference;}
    public DateTime? Eta {get => _eta;}
    public int AllocatedQuantity {get => _allocations.Sum(x => x.Quantity);}
    public int AvailableQuantity {get => _purchasedQuantity - AllocatedQuantity;}

    public Batch(string reference, string sku, int quantity, DateTime? eta)
    {
        _reference = reference;
        _sku = sku;
        _purchasedQuantity = quantity;
        _eta = eta;
        _allocations = new List<OrderLine>();
    }

    public void Allocate(OrderLine orderLine)
    {
        if (!CanAllocate(orderLine))
        {
            return ;
        }

        _allocations.Add(orderLine);
    }

    public void Deallocate(OrderLine orderLine)
    {
        if (!CanDeallocate(orderLine))
        {
            return ;
        }

        _allocations.Remove(orderLine);
    }

    public bool CanAllocate(OrderLine orderLine)
    {
        return _sku == orderLine.Sku 
            && _purchasedQuantity >= orderLine.Quantity
            && _allocations.Where(x => x.Equals(orderLine)).Count() == 0;
    }

    public bool CanDeallocate(OrderLine orderLine)
    {
        return _allocations.Where(x => x.Equals(orderLine)).Count() > 0;
    }

    public override bool Equals(object? obj)
    {
        var batch = obj as Batch;

        if (batch == null)
        {
            return false;
        }

        return batch._reference == _reference;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

namespace Domain;

public class RequiresQuantityGreaterThanAvailableException : Exception
{
    public RequiresQuantityGreaterThanAvailableException()
    : base("Cannot allocate to a batch if the available quantity is less than the quantity of the order line")
    {
    }
}

public class DuplicateOrderLineException : Exception
{
    public DuplicateOrderLineException()
    : base("Cannot allocate the same order line twice")
    {
    }
}

public class SkuDoesNotMatchException : Exception
{
    public SkuDoesNotMatchException()
    : base("Cannot allocate if order line SKU is different from batch SKU")
    {
    }
}

public class UnallocatedOrderLineException : Exception
{
    public UnallocatedOrderLineException()
    : base("Cannot deallocate an unallocated order line")
    {
    }
}

public class OrderLine
{
    private string _orderId;
    private string _sku;
    private int _quantity;

    public string Sku {get => _sku;}
    public int Quantity {get => _quantity;}

    public OrderLine(string orderId, string sku, int quantity)
    {
        _orderId = orderId;
        _sku = sku;
        _quantity = quantity;
    }

    public override bool Equals(object? obj)
    {
        var orderLine = obj as OrderLine;

        if (orderLine == null)
        {
            return false;
        }

        return orderLine._orderId == _orderId
            && orderLine._sku == _sku
            && orderLine._quantity == _quantity;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class Batch
{
    private string _reference;
    private string _sku;
    private int _purchasedQuantity;
    private DateTime? _eta;
    private List<OrderLine> _allocations;

    public int AllocatedQuantity {get => _allocations.Sum(x => x.Quantity);}
    public int AvailableQuantity {get => _purchasedQuantity - AllocatedQuantity;}

    public Batch(string reference, string sku, int quantity)
    : this(reference, sku, quantity, null, new List<OrderLine>())
    {
    }

    public Batch(string reference, string sku, int quantity, List<OrderLine> allocations)
    : this(reference, sku, quantity, null, allocations)
    {
    }

    public Batch(string reference, string sku, int quantity, DateTime? eta)
    : this(reference, sku, quantity, eta, new List<OrderLine>())
    {
    }

    public Batch(string reference, string sku, int quantity, DateTime? eta, List<OrderLine> allocations)
    {
        _reference = reference;
        _sku = sku;
        _purchasedQuantity = quantity;
        _eta = eta;
        _allocations = allocations;
    }

    private void ValidateOrderLineRequirements(OrderLine orderLine)
    {
        if (orderLine.Quantity > AvailableQuantity)
        {
            throw new RequiresQuantityGreaterThanAvailableException();
        }

        if (_allocations.Where(x => x.Equals(orderLine)).Count() > 0)
        {
            throw new DuplicateOrderLineException();
        }

        if (orderLine.Sku != _sku)
        {
            throw new SkuDoesNotMatchException();
        }
    }

    public bool CanAllocate(OrderLine orderLine)
    {
        try
        {
            ValidateOrderLineRequirements(orderLine);

            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public void Allocate(OrderLine orderLine)
    {
        ValidateOrderLineRequirements(orderLine);

        _allocations.Add(orderLine);
    }

    public void Deallocate(OrderLine orderLine)
    {
        if (_allocations.Where(x => x.Equals(orderLine)).Count() == 0) {
            throw new UnallocatedOrderLineException();
        }

        _allocations.Remove(orderLine);
    }
}

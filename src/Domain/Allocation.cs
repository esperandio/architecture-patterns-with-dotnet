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

public class OutOfStockException : Exception
{
    public OutOfStockException(string sku)
    : base($"Out of stock for sku {sku}") 
    {
    }
}

public class OrderLine
{
    public string OrderId {get; private set;}
    public string Sku {get; private set;}
    public int Quantity {get; private set;}

    public OrderLine(string orderId, string sku, int quantity)
    {
        OrderId = orderId;
        Sku = sku;
        Quantity = quantity;
    }

    public override bool Equals(object? obj)
    {
        var orderLine = obj as OrderLine;

        if (orderLine == null)
        {
            return false;
        }

        return orderLine.OrderId == OrderId
            && orderLine.Sku == Sku
            && orderLine.Quantity == Quantity;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class Batch
{
    private readonly List<OrderLine> _allocations;

    public string Reference { get; private set; }
    public string Sku { get; private set; }
    public int PurchasedQuantity { get; private set; }
    public DateTime? Eta {get; private set;}

    public IReadOnlyCollection<OrderLine> Allocations => _allocations;
    public int AllocatedQuantity => _allocations.Sum(x => x.Quantity);
    public int AvailableQuantity => PurchasedQuantity - AllocatedQuantity;

    public Batch(string reference, string sku, int purchasedQuantity)
    : this(reference, sku, purchasedQuantity, null, new List<OrderLine>())
    {
    }

    public Batch(string reference, string sku, int purchasedQuantity, List<OrderLine> allocations)
    : this(reference, sku, purchasedQuantity, null, allocations)
    {
    }

    public Batch(string reference, string sku, int purchasedQuantity, DateTime? eta)
    : this(reference, sku, purchasedQuantity, eta, new List<OrderLine>())
    {
    }

    public Batch(string reference, string sku, int purchasedQuantity, DateTime? eta, List<OrderLine> allocations)
    {
        Reference = reference;
        Sku = sku;
        PurchasedQuantity = purchasedQuantity;
        Eta = eta;
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

        if (orderLine.Sku != Sku)
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

    public override bool Equals(object? obj)
    {
        var batch = obj as Batch;

        if (batch == null)
        {
            return false;
        }

        return batch.Reference == Reference;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class AllocationService
{
    public static void Allocate(OrderLine orderLine, List<Batch> batches)
    {
        var availableBatch = batches
            .Where(x => x.CanAllocate(orderLine))
            .OrderBy(x => x.Eta)
            .FirstOrDefault();
        
        if (availableBatch == null) 
        {
            throw new OutOfStockException(orderLine.Sku);
        }

        availableBatch.Allocate(orderLine);
    }
}

namespace Domain;

public class OutOfStockException : Exception
{
    public OutOfStockException()
    : base("Cannot allocate to a batch if the available quantity is less than the quantity of the order line")
    {
    }
}

public class AllocateSameLineTwiceException : Exception
{
    public AllocateSameLineTwiceException()
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
    private int _allocatedQuantity;
    private DateTime? _eta;
    private List<OrderLine> _allocations;

    public int AvailableQuantity {get => _purchasedQuantity - _allocatedQuantity;}

    public Batch(string reference, string sku, int quantity)
    : this(reference, sku, quantity, null)
    {
    }

    public Batch(string reference, string sku, int quantity, DateTime? eta)
    {
        _reference = reference;
        _sku = sku;
        _purchasedQuantity = quantity;
        _eta = eta;
        _allocations = new List<OrderLine>();
    }

    public void allocate(OrderLine orderLine)
    {
        if (orderLine.Quantity > AvailableQuantity)
        {
            throw new OutOfStockException();
        }

        if (_allocations.Where(x => x.Equals(orderLine)).Count() > 0)
        {
            throw new AllocateSameLineTwiceException();
        }

        if (orderLine.Sku != _sku)
        {
            throw new SkuDoesNotMatchException();
        }

        _allocations.Add(orderLine);
        _allocatedQuantity += orderLine.Quantity;
    }
}

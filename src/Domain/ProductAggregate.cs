namespace Domain;

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
    public int PurchasedQuantity { get; set; }
    public DateTime? Eta {get; private set;}

    public IReadOnlyCollection<OrderLine> Allocations => _allocations.AsReadOnly();
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

    public bool CanAllocate(OrderLine orderLine)
    {
        if (orderLine.Quantity > AvailableQuantity)
        {
            return false;
        }

        return true;
    }

    public bool HasOrderLine(OrderLine orderLine)
    {
        return _allocations.Where(x => x.Equals(orderLine)).Count() > 0;
    }

    public void Allocate(OrderLine orderLine)
    {
        if (!CanAllocate(orderLine))
        {
            return;
        }

        _allocations.Add(orderLine);
    }

    public void Deallocate(OrderLine orderLine)
    {
        _allocations.Remove(orderLine);
    }

    public OrderLine DeallocateOne()
    {
        var orderLine = _allocations.First();

        Deallocate(orderLine);

        return orderLine;
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

public class Product
{
    private readonly List<Batch> _batches;
    private readonly List<IMessage> _domainEvents;

    public string Sku { get; private set; }
    public Guid ?Version { get; private set; }
    public IReadOnlyCollection<Batch> Batches => _batches.AsReadOnly();
    public IReadOnlyCollection<IMessage> DomainEvents => _domainEvents.AsReadOnly();

    public Product(string sku)
    : this(sku, new List<Batch>())
    {
    }

    public Product(string sku, List<Batch> batches)
    {
        Sku = sku;
        _batches = batches;
        _domainEvents = new List<IMessage>();
    }

    private bool IsOrderLineAlreadyAllocated(OrderLine orderLine)
    {
        var batch = _batches.FirstOrDefault(x => x.HasOrderLine(orderLine));

        if (batch == null)
        {
            return false;
        }

        return true;
    }

    public string Allocate(string orderId, string sku, int quantity)
    {
        if (sku != Sku)
        {
            throw new SkuDoesNotMatchException();
        }

        var orderline = new OrderLine(orderId, sku, quantity);

        if (IsOrderLineAlreadyAllocated(orderline))
        {
            throw new DuplicateOrderLineException();
        }

        var batch = _batches
            .Where(x => x.CanAllocate(orderline))
            .OrderBy(x => x.Eta)
            .FirstOrDefault();

        if (batch == null)
        {
            _domainEvents.Add(new OutOfStockEvent(sku));
            return String.Empty;
        }
        
        batch.Allocate(orderline);

        _domainEvents.Add(new AllocatedEvent(orderline.OrderId, orderline.Sku, orderline.Quantity, batch.Reference));

        Version = Guid.NewGuid();

        return batch.Reference;
    }

    public string AddBatch(string reference, string sku, int purchasedQuantity, DateTime? eta)
    {
        var batch = new Batch(reference, sku, purchasedQuantity, eta);

        _batches.Add(batch);

        Version = Guid.NewGuid();

        return batch.Reference; 
    }

    public void ChangeBatchQuantity(string reference, int quantity)
    {
        var batch = _batches.FirstOrDefault(x => x.Reference == reference);

        if (batch == null)
        {
            return;
        }
        
        batch.PurchasedQuantity = quantity;

        while (batch.AvailableQuantity < 0)
        {
            var orderLine = batch.DeallocateOne();

            _domainEvents.Add(
                new AllocateCommand(
                    orderLine.OrderId,
                    orderLine.Sku,
                    orderLine.Quantity
                )
            );
        }

        Version = Guid.NewGuid();
    }

    public int BatchAvailableQuantity(string reference)
    {
        var batch = _batches.FirstOrDefault(x => x.Reference == reference);

        if (batch == null)
        {
            return 0;
        }

        return batch.AvailableQuantity;
    }
}

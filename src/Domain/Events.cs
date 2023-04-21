namespace Domain;

public class Event : IMessage
{
}

public class OutOfStockEvent : Event
{
    public string Sku { get; private set; }

    public OutOfStockEvent(string sku)
    {
        Sku = sku;
    }

    public override bool Equals(object? obj)
    {
        var @event = obj as OutOfStockEvent;

        if (@event == null)
        {
            return false;
        }

        return Sku == @event.Sku;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class BatchCreatedEvent : Event
{
    public string Reference { get; private set; }
    public string Sku { get; private set; }
    public int PurchasedQuantity { get; private set; }
    public DateTime? Eta { get; private set; }

    public BatchCreatedEvent(string reference, string sku, int purchasedQuantity)
    : this(reference, sku, purchasedQuantity, null)
    {
    }

    public BatchCreatedEvent(string reference, string sku, int purchasedQuantity, DateTime? eta)
    {
        Reference = reference;
        Sku = sku;
        PurchasedQuantity = purchasedQuantity;
        Eta = eta;
    }
}

public class AllocationRequiredEvent : Event
{
    public string OrderId { get; private set; }
    public string Sku { get; private set; }
    public int Qty { get; private set; }

    public AllocationRequiredEvent(string orderId, string sku, int qty)
    {
        OrderId = orderId;
        Sku = sku;
        Qty = qty;
    }
}

public class BatchQuantityChangedEvent : Event
{
    public string Reference { get; private set; }
    public int Quantity { get; private set; }

    public BatchQuantityChangedEvent(string reference, int quantity)
    {
        Reference = reference;
        Quantity = quantity;
    }
}

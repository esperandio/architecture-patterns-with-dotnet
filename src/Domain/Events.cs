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

public class AllocatedEvent : Event
{
    public string OrderId { get; private set; }
    public string Sku { get; private set; }
    public int Qty { get; private set; }
    public string BatchReference { get; private set; }

    public AllocatedEvent(string orderId, string sku, int qty, string batchReference)
    {
        OrderId = orderId;
        Sku = sku;
        Qty = qty;
        BatchReference = batchReference;
    }

    public override bool Equals(object? obj)
    {
        var @event = obj as AllocatedEvent;

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

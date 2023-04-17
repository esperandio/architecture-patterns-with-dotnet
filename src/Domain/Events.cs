namespace Domain;

public class Event
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

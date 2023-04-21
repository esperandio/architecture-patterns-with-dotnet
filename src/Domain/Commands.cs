namespace Domain;

public class Command : IMessage
{
}

public class AllocateCommand : Command
{
    public string OrderId { get; private set; }
    public string Sku { get; private set; }
    public int Qty { get; private set; }

    public AllocateCommand(string orderId, string sku, int qty)
    {
        OrderId = orderId;
        Sku = sku;
        Qty = qty;
    }
}

public class CreateBatchCommand : Command
{
    public string Reference { get; private set; }
    public string Sku { get; private set; }
    public int PurchasedQuantity { get; private set; }
    public DateTime? Eta { get; private set; }

    public CreateBatchCommand(string reference, string sku, int purchasedQuantity)
    : this(reference, sku, purchasedQuantity, null)
    {
    }

    public CreateBatchCommand(string reference, string sku, int purchasedQuantity, DateTime? eta)
    {
        Reference = reference;
        Sku = sku;
        PurchasedQuantity = purchasedQuantity;
        Eta = eta;
    }
}

public class ChangeBatchQuantityCommand : Command
{
    public string Reference { get; private set; }
    public int Quantity { get; private set; }

    public ChangeBatchQuantityCommand(string reference, int quantity)
    {
        Reference = reference;
        Quantity = quantity;
    }
}

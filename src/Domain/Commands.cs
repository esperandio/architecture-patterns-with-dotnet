namespace Domain;

public class Command : IMessage
{
}

public class AllocateCommand : Command
{
    public string OrderId { get; set; }
    public string Sku { get; set; }
    public int Qty { get; set; }

    public AllocateCommand()
    : this("", "", 0)
    {
    }

    public AllocateCommand(string orderId, string sku, int qty)
    {
        OrderId = orderId;
        Sku = sku;
        Qty = qty;
    }
}

public class CreateBatchCommand : Command
{
    public string Reference { get; set; }
    public string Sku { get; set; }
    public int PurchasedQuantity { get; set; }
    public DateTime? Eta { get; set; }

    public CreateBatchCommand()
    : this("", "", 0)
    {
    }

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
    public string Reference { get; set; }
    public int Quantity { get; set; }

    public ChangeBatchQuantityCommand()
    : this("", 0)
    {
    }

    public ChangeBatchQuantityCommand(string reference, int quantity)
    {
        Reference = reference;
        Quantity = quantity;
    }
}

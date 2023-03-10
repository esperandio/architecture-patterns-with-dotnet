namespace Domain;

public class OrderLine
{
    private string _sku;
    private int _quantity;
    public int Quantity {get => _quantity;}

    public OrderLine(string sku, int quantity)
    {
        _sku = sku;
        _quantity = quantity;
    }
}

public class Batch
{
    private string _reference;
    private string _sku;
    private int _purchasedQuantity;
    private int _allocatedQuantity;
    private DateTime? _eta;
    public int AvailableQuantity {get => _purchasedQuantity - _allocatedQuantity;}

    public Batch(string reference, string sku, int quantity, DateTime? eta)
    {
        _reference = reference;
        _sku = sku;
        _purchasedQuantity = quantity;
        _eta = eta;
    }

    public void allocate(OrderLine orderLine)
    {
        _allocatedQuantity += orderLine.Quantity;
    }
}

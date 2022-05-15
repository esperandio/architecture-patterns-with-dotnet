namespace Domain;

public class OrderLine
{
    private string _orderId;
    private int _quantity;
    private string _sku;
    public string Sku {get => _sku;}
    public int Quantity {get => _quantity;}

    public OrderLine(string orderId, string sku, int quantity)
    {
        _orderId = orderId;
        _sku = sku;
        _quantity = quantity;
    }
}

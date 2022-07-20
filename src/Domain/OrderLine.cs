namespace Domain;

public class OrderLine
{
    private int _id;
    private string _orderId;
    private int _quantity;
    private string _sku;
    public virtual int Id
    {
        get
        {
            return _id;
        }
        protected set
        {
            _id = value;
        }
    }
    public string OrderId {get => _orderId;}
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
            && orderLine._quantity == _quantity
            && orderLine._sku == _sku;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

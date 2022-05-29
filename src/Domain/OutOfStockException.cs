namespace Domain;

public class OutOfStockException : Exception
{
    public OutOfStockException(string sku): base($"Out of stock for sku {sku}") 
    {
    }
}
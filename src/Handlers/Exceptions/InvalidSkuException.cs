namespace Handlers.Exceptions;

public class InvalidSkuException : Exception
{
    public InvalidSkuException(string sku)
    : base($"Unable to find product. SKU: {sku}")
    {
    }
}
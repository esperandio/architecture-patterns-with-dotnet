namespace Infrastructure.Persistence.Models;

class OrderLine
{
    public int ID { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; }

    public Order Order { get; set; }
}
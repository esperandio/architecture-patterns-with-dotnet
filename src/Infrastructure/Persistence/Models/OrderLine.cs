namespace Infrastructure.Persistence.Models;

public class OrderLine : EntityModel
{
    public int ID { get; set; }
    public string OrderId { get; set; }
    public int Quantity { get; set; }
    public string Sku { get; set; }
}
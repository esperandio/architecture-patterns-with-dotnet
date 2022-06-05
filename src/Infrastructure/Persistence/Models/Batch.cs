namespace Infrastructure.Persistence.Models;

public class Batch
{
    public int ID { get; set; }
    public string Reference { get; set; }
    public string Sku { get; set; }
    public int PurchasedQuantity { get; set; }
    public DateTime Eta { get; set; }
}
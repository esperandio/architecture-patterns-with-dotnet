namespace Infrastructure.Persistence.Models;

public class Batch : EntityModel
{
    public int ID { get; set; }
    public string Reference { get; set; }
    public string Sku { get; set; }
    public int PurchasedQuantity { get; set; }
    public DateTime? Eta { get; set; }

    public ICollection<Allocation> Allocations { get; set; }
}
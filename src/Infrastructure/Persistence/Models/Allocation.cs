namespace Infrastructure.Persistence.Models;

public class Allocation : EntityModel
{
    public int ID { get; set; }
    public int OrderLineId { get; set; }
    public int BatchId { get; set; }

    public OrderLine OrderLine { get; set; }
    public Batch Batch { get; set; }
}
namespace Infrastructure.Persistence.Models;

public class Order : EntityModel
{
    public int ID { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; }
}
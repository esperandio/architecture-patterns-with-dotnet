namespace Infrastructure.Persistence.Models;

public class Order
{
    public int ID { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; }
}
namespace Infrastructure.Persistence.Models;

class Order
{
    public int ID { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; }
}
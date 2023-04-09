namespace Infrastructure.Data.Tests;

using Domain;

public class AppDbContextTest : IDisposable
{
    private AppDbContext _context;

    public AppDbContextTest()
    {
        _context =  new AppDbContext();
    }

    public void Dispose()
    {
        _context.Database.ExecuteSqlRaw("DELETE FROM Products");
    }

    [Fact]
    public void TestCanRetrieveBatches()
    {
        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Products (Sku) VALUES ('SMALL-TABLE')"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Batches (Reference, Sku, PurchasedQuantity) VALUES ('batch-001', 'SMALL-TABLE', 20)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Batches (Reference, Sku, PurchasedQuantity) VALUES ('batch-002', 'SMALL-TABLE', 20)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Batches (Reference, Sku, PurchasedQuantity) VALUES ('batch-003', 'SMALL-TABLE', 20)"
        );

        _context.SaveChanges();

        var product = _context.Products.Find("SMALL-TABLE");

        Assert.Equal(
            new List<Batch>() 
            { 
                new Batch("batch-001", "SMALL-TABLE", 20),
                new Batch("batch-002", "SMALL-TABLE", 20), 
                new Batch("batch-003", "SMALL-TABLE", 20)
            },
            product?.Batches.ToList()
        );
    }

    [Fact]
    public void TestCanRetrieveSpecificBatch()
    {
        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Products (Sku) VALUES ('SMALL-TABLE')"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Batches (Reference, Sku, PurchasedQuantity) VALUES ('batch-001', 'SMALL-TABLE', 20)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLines (Reference, OrderId, Sku, Quantity) VALUES ('batch-001', 'order-001','SMALL-TABLE', 5)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLines (Reference, OrderId, Sku, Quantity) VALUES ('batch-001', 'order-002','SMALL-TABLE', 5)"
        );

        var product = _context.Products.Find("SMALL-TABLE");

        Assert.Equal(
            new List<OrderLine>
            {
                new OrderLine("order-001", "SMALL-TABLE", 5),
                new OrderLine("order-002", "SMALL-TABLE", 5)
            },
            product?.Batches.First(x => x.Reference == "batch-001").Allocations
        );
    }

    [Fact]
    public void TestCanPersistNewBatch()
    {
        var batch = new Batch(
            "batch-001", 
            "BLUE-VASE", 
            10, 
            new List<OrderLine>() 
            { 
                new OrderLine("order-001", "BLUE-VASE", 2), 
                new OrderLine("order-002", "BLUE-VASE", 2) 
            }
        );

        _context.Products.Add(
            new Product("BLUE-VASE", new List<Batch>(){ batch })
        );

        _context.SaveChanges();

        var product = _context.Products.Find("BLUE-VASE");

        Assert.Equal(1, product?.Batches.Count());
        Assert.Equal(4, product?.Batches.First(x => x.Reference == "batch-001").AllocatedQuantity);
    }

    [Fact]
    public void TestCanModifyAnExistingBatchAndPersist()
    {
        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Products (Sku) VALUES ('SMALL-TABLE')"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Batches (Reference, Sku, PurchasedQuantity) VALUES ('batch-001', 'SMALL-TABLE', 20)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLines (Reference, OrderId, Sku, Quantity) VALUES ('batch-001', 'order-001','SMALL-TABLE', 5)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLines (Reference, OrderId, Sku, Quantity) VALUES ('batch-001', 'order-002','SMALL-TABLE', 5)"
        );

        var product = _context.Products.Find("SMALL-TABLE");

        if (product == null)
        {
            return;
        }

        product.Allocate(new OrderLine("order-003", "SMALL-TABLE", 8));

        _context.Products.Update(product);

        _context.SaveChanges();

        Assert.Equal(
            18, 
            _context.Products.Find("SMALL-TABLE")?.Batches
                .First(x => x.Reference == "batch-001")
                .AllocatedQuantity
        );
    }
}
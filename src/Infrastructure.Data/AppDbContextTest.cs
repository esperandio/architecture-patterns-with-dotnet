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
        _context.Database.ExecuteSqlRaw("DELETE FROM Batches");
    }

    [Fact]
    public void TestCanRetrieveBatches()
    {
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

        Assert.Equal(
            new List<Batch>() 
            { 
                new Batch("batch-001", "SMALL-TABLE", 20),
                new Batch("batch-002", "SMALL-TABLE", 20), 
                new Batch("batch-003", "SMALL-TABLE", 20)
            },
            _context.Batches.ToList()
        );
    }

    [Fact]
    public void TestCanRetrieveSpecificBatch()
    {
        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Batches (Reference, Sku, PurchasedQuantity) VALUES ('batch-001', 'SMALL-TABLE', 20)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLine (Id, BatchReference, OrderId, Sku, Quantity) VALUES (1, 'batch-001', 'order-001','SMALL-TABLE', 5)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLine (Id, BatchReference, OrderId, Sku, Quantity) VALUES (2, 'batch-001', 'order-002','SMALL-TABLE', 5)"
        );

        Assert.Equal(
            new List<OrderLine>
            {
                new OrderLine("order-001", "SMALL-TABLE", 5),
                new OrderLine("order-002", "SMALL-TABLE", 5)
            },
            _context.Batches.Find("batch-001")?.Allocations
        );
    }

    [Fact]
    public void TestCanPersistNewBatch()
    {
        _context.Batches.Add(
            new Batch(
                "batch-001", 
                "BLUE-VASE", 
                10, 
                new List<OrderLine>() 
                { 
                    new OrderLine("order-001", "BLUE-VASE", 2), 
                    new OrderLine("order-002", "BLUE-VASE", 2) 
                }
            )
        );

        _context.SaveChanges();

        Assert.Equal(1, _context.Batches.Count());
        Assert.Equal(4, _context.Batches.Find("batch-001")?.AllocatedQuantity);
    }

    [Fact]
    public void TestCanModifyAnExistingBatchAndPersist1()
    {
        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Batches (Reference, Sku, PurchasedQuantity) VALUES ('batch-001', 'SMALL-TABLE', 20)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLine (Id, BatchReference, OrderId, Sku, Quantity) VALUES (1, 'batch-001', 'order-001','SMALL-TABLE', 5)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLine (Id, BatchReference, OrderId, Sku, Quantity) VALUES (2, 'batch-001', 'order-002','SMALL-TABLE', 5)"
        );

        var existingBatch = _context.Batches.Find("batch-001");

        if (existingBatch == null)
        {
            return;
        }

        existingBatch.Allocate(new OrderLine("order-003", "SMALL-TABLE", 8));

        _context.Batches.Update(existingBatch);

        _context.SaveChanges();

        Assert.Equal(18, _context.Batches.Find("batch-001")?.AllocatedQuantity);
    }

    [Fact]
    public void TestCanModifyAnExistingBatchAndPersist2()
    {
        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Batches (Reference, Sku, PurchasedQuantity) VALUES ('batch-001', 'SMALL-TABLE', 20)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLine (Id, BatchReference, OrderId, Sku, Quantity) VALUES (1, 'batch-001', 'order-001','SMALL-TABLE', 5)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLine (Id, BatchReference, OrderId, Sku, Quantity) VALUES (2, 'batch-001', 'order-002','SMALL-TABLE', 5)"
        );

        var existingBatch = _context.Batches.Find("batch-001");

        if (existingBatch == null)
        {
            return;
        }

        existingBatch.Deallocate(new OrderLine("order-002", "SMALL-TABLE", 5));

        _context.Batches.Update(existingBatch);

        _context.SaveChanges();

        Assert.Equal(5, _context.Batches.Find("batch-001")?.AllocatedQuantity);
    }

    [Fact]
    public void TestCanModifyAnExistingBatchAndPersist3()
    {
        _context.Database.ExecuteSqlRaw(
            "INSERT INTO Batches (Reference, Sku, PurchasedQuantity) VALUES ('batch-001', 'SMALL-TABLE', 20)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLine (Id, BatchReference, OrderId, Sku, Quantity) VALUES (1, 'batch-001', 'order-001','SMALL-TABLE', 5)"
        );

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO OrderLine (Id, BatchReference, OrderId, Sku, Quantity) VALUES (2, 'batch-001', 'order-002','SMALL-TABLE', 5)"
        );

        var existingBatch = _context.Batches.Find("batch-001");

        if (existingBatch == null)
        {
            return;
        }

        existingBatch.Deallocate(new OrderLine("order-002", "SMALL-TABLE", 5));
        existingBatch.Allocate(new OrderLine("order-003", "SMALL-TABLE", 8));

        _context.Batches.Update(existingBatch);

        _context.SaveChanges();

        Assert.Equal(13, _context.Batches.Find("batch-001")?.AllocatedQuantity);
    }
}
namespace Infrastructure.Data.Tests;

// Necessário inserir as seguintes pendências para que o teste funcione:
// INSERT INTO Products (Sku, Version) VALUES('SMALL-TABLE', '85f9c099-f269-47e8-b1b1-1c4402c13d6c');
// INSERT INTO Batches (Reference, Sku, PurchasedQuantity, Eta) VALUES('batch001', 'SMALL-TABLE', 20, NULL);

public class TestTransaction1
{
    private UnitOfWork uow;

    public TestTransaction1()
    {
        var context =  new AppDbContext();

        uow = new UnitOfWork(context, new ProductRepository(context));
    }

    [Fact(Skip = "Teste usado somente para validar concorrência de alocação")]
    public async void TestConcurrentUpdate1()
    {
        var product = await uow.Products.Get("SMALL-TABLE");

        if (product == null)
        {
            return;
        }

        product.Allocate("order-001", "SMALL-TABLE", 4);

        Thread.Sleep(3000);

        await uow.Commit();
    }
}

public class TestTransaction2
{
    private UnitOfWork uow;

    public TestTransaction2()
    {
        var context =  new AppDbContext();

        uow = new UnitOfWork(context, new ProductRepository(context));
    }

    [Fact(Skip = "Teste usado somente para validar concorrência de alocação")]
    public async void TestConcurrentUpdate2()
    {
        var product = await uow.Products.Get("SMALL-TABLE");

        if (product == null)
        {
            return;
        }

        product.Allocate("order-002", "SMALL-TABLE", 6);

        Thread.Sleep(8000);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => { 
            await uow.Commit();
        });
    }
}

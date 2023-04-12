namespace Domain.Tests;

public class AllocationTest
{
    [Fact]
    public void TestCannotAllocateIfSkusDoNotMatch()
    {
        var product = new Product("SMALL-FORK");

        Assert.Throws<SkuDoesNotMatchException>(() => { 
            product.Allocate("order-001", "EXPENSIVE-TOASTER", 2);
        });
    }
}

using Dapper;
using MySqlConnector;

namespace Queries;

public class AllocationQueries
{
    public async Task<IEnumerable<Allocation>> GetAllocations(string orderId)
    {
        string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? "";
        var connection = new MySqlConnection(connectionString);

        var result = await connection.QueryAsync<Allocation>($"SELECT ol.Sku, ol.Reference FROM app.OrderLines ol WHERE ol.OrderId = '{orderId}'");

        if (result.Count() == 0)
        {
            throw new KeyNotFoundException();
        }

        return result;
    }
}

public record Allocation
{
    public string sku { get; set; } = "";
    public string reference { get; set; } = "";
}
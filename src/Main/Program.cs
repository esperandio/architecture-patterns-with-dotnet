using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Infrastructure.Persistence;
using Domain;
using Main.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Get environment
string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Build config
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddPersistenceServices(config);

builder.Services.AddScoped<AllocateUseCase>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/batch/{id}", async (ApplicationDbContext context, int id) => {
    var batch = await context.Batches.FindAsync(id);
    await context.Entry(batch).Collection(x => x.Allocations).LoadAsync();

    return Results.Ok(batch);
});

app.MapPost("/allocate", (AllocateUseCase useCase, AllocateRequestData request) => {
    try
    {
        var reference = useCase.Perform(new AllocateData()
        {
            OrderId = request.OrderId,
            Sku = request.Sku,
            Qty = request.Qty
        });

        return Results.Ok(reference);
    }
    catch (OutOfStockException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

class AllocateRequestData
{
    public string OrderId { get; set; }
    public string Sku { get; set; }
    public int Qty { get; set; }
}
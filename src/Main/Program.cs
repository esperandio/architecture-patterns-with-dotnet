using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Models;

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

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/batches/count", (EntityFrameworkRepository repository) => repository.Count<Batch>());
app.MapGet("/batch/{id}", async (EntityFrameworkRepository repository, int id) => {
    var batch = await repository.GetBatchById(id);
    return Results.Ok(batch);
});

app.Run();

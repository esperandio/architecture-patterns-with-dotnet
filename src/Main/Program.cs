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

builder.Services.AddPersistenceServices(config);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/batches/count", (EntityFrameworkRepository repository) => repository.Count<Batch>());

app.Run();

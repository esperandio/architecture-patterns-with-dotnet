using Infrastructure.Persistence;

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
app.MapGet("/batches/count", (ApplicationDbContext applicationDbContext) => applicationDbContext.Batches.Count());

app.Run();

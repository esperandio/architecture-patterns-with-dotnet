using UseCases;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDataService();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/allocate", async (AppDbContext dbContext, AllocateData request) => {
    try
    {
        var repository = new BatchRepository(dbContext);

        var reference = await new AllocateUseCase(repository).Perform(request);

        await dbContext.SaveChangesAsync();

        return Results.Ok(reference);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/batch", async (AppDbContext dbContext, AddBatchData request) => {
    try
    {
        var repository = new BatchRepository(dbContext);

        var reference = await new AddBatchUseCase(repository).Perform(request);

        await dbContext.SaveChangesAsync();

        return Results.Ok(reference);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

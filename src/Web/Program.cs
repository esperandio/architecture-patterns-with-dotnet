using UseCases;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDataService();

builder.Services.AddScoped<AllocateUseCase>();
builder.Services.AddScoped<AddBatchUseCase>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/allocate", async (AllocateUseCase useCase, AllocateData request) => {
    try
    {
        var reference = await useCase.Perform(request);

        return Results.Ok(reference);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/batch", async (AddBatchUseCase useCase, AddBatchData request) => {
    try
    {
        var reference = await useCase.Perform(request);

        return Results.Ok(reference);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

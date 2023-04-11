using UseCases;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDataService();

builder.Services.AddScoped<AllocateUseCase>();
builder.Services.AddScoped<AddBatchUseCase>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

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

app.MapPost("/allocate/{batchReference}", async (AllocateUseCase useCase, string batchReference, AllocateData request) => {
    try
    {
        var reference = await useCase.Perform(batchReference, request);

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

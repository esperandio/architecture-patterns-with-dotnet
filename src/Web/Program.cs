using Handlers;
using Infrastructure.Data;
using Infrastructure.Mail;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDataService();
builder.Services.AddInfrastructureMailService();

builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddScoped<Allocate>();
builder.Services.AddScoped<AddBatch>();

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

app.MapPost("/allocate", async (Allocate useCase, AllocateData request) => {
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

app.MapPost("/allocate/{batchReference}", async (Allocate useCase, string batchReference, AllocateData request) => {
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

app.MapPost("/batch", async (AddBatch useCase, AddBatchData request) => {
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

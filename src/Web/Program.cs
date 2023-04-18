using Domain;
using Handlers;
using Infrastructure.Data;
using Infrastructure.Mail;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDataService();
builder.Services.AddInfrastructureMailService();

builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddScoped<AllocateHandler>();
builder.Services.AddScoped<AddBatchHandler>();

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

app.MapPost("/allocate", async (AllocateHandler handler, AllocateData request) => {
    try
    {
        var reference = await handler.Handle(request);

        return Results.Ok(reference);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/allocate/{batchReference}", async (AllocateHandler handler, string batchReference, AllocateData request) => {
    try
    {
        var reference = await handler.Handle(batchReference, request);

        return Results.Ok(reference);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/batch", async (AddBatchHandler handler, BatchCreatedEvent request) => {
    try
    {
        var reference = await handler.Handle(request);

        return Results.Ok(reference);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

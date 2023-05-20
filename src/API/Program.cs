using Domain;
using Handlers;
using Handlers.Abstractions;
using Infrastructure.Data;
using Infrastructure.Mail;
using Infrastructure.MessageBroker;
using Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDataService();
builder.Services.AddInfrastructureMailService();
builder.Services.AddInfrastructureMessageBrokerService();

builder.Services.AddScoped<IMessageBus, MessageBus>();

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

app.MapGet("/healthcheck", () => {
    return Results.Ok("ok");
});

app.MapPost("/allocate", async (IMessageBus messageBus, AllocateCommand request) => {
    try
    {
        await messageBus.Handle(request);

        return Results.Accepted();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/batch", async (IMessageBus messageBus, CreateBatchCommand request) => {
    try
    {
        await messageBus.Handle(request);

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/allocations/{orderId}", async (string orderId) => {
    try
    {
        var queries = new AllocationQueries();
        var allocations = await queries.GetAllocations(orderId);

        return Results.Ok(allocations);
    }
    catch (Exception)
    {
        return Results.NotFound();
    }
});

app.Run();

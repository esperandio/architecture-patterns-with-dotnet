using Domain;
using Handlers;
using Infrastructure.Data;
using Infrastructure.Mail;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureDataService();
builder.Services.AddInfrastructureMailService();

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

app.MapPost("/allocate", async (IMessageBus messageBus, AllocateCommand request) => {
    try
    {
        await messageBus.Handle(request);

        var reference = messageBus.Results.Last();

        return Results.Ok(reference);
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

        var reference = messageBus.Results.Last();

        return Results.Ok(reference);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

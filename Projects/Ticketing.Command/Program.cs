using MediatR;
using Ticketing.Command.Application;
using Ticketing.Command.Features.Apis;
using Ticketing.Command.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.RegisterMinimalApis();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// app.MapPost("/api/ticket", async (
//     IMediator mediator,
//     TicketCreateRequest request,
//     CancellationToken cancellationToken
// ) =>
// {
//     var command = new TicketCreateCommand(request);
//     var result = await mediator.Send(command, cancellationToken);
//     return Results.Ok(result);
// })
// .WithName("CreateTicket");

app.MapMinimalApisEndpoints();
app.Run();

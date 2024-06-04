using Microsoft.AspNetCore.SignalR;
using WebApplication1;
using WebApplication1.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


var app  = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("broadcast", async (string message, IHubContext<ChatHub, IChatClient> context) =>
{
    await context.Clients.All.RecieveMessage(message);

    return Results.NoContent();
});
app.MapHub<ChatHub>("/chat");

app.Run();
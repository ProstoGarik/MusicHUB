using Microsoft.AspNetCore.SignalR;
using WebApplication1;
using WebApplication1.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://+:8080");
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(configure => { configure.MaximumReceiveMessageSize = null; });

var app  = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<MainHub>("/chat");

app.Run();
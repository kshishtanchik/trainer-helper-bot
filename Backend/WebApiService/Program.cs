using DatabaseService.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables("SECRET");
var cfg = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddLogging(opt =>
{
    opt.AddSimpleConsole();
    opt.AddJsonConsole();
});

builder.Services.AddDatabaseService(cfg.GetConnectionString("Default"));
builder.Services.AddTelegramBot(cfg.GetSection(nameof(BotSettings)).Get<BotSettings>());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
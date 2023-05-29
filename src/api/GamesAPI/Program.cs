using CodeBreaker.APIs.Endpoints;
using CodeBreaker.APIs.Services;
using CodeBreaker.Data;
using CodeBreaker.Shared.Api;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddDbContext<ICodeBreakerRepository, CodeBreakerContext>(options =>
{
    var connection = builder.Configuration["CosmosConnection"] ?? throw new InvalidOperationException("Connection not found");
    options.UseCosmos(connection, "thrivesample2023");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

//app.MapPost("/games", ([FromBody] CreateGameRequest request,  [FromServices] IGameService gameService) =>
//{

//});

app.MapGameEndpoints(app.Logger);

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

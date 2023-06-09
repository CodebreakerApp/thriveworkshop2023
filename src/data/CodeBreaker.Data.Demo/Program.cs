﻿using Azure.Identity;

using CodeBreaker.Data;
using CodeBreaker.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

using static CodeBreaker.Shared.Colors;

string cosmosEndpoint = "https://thrive2023.documents.azure.com:443/";
string databaseName = "thrivesample2023";
// DefaultAzureCredential azureCredential = new();
AzureCliCredential azureCredential = new();

var contextOptions = new DbContextOptionsBuilder<CodeBreakerContext>()
    .UseCosmos(cosmosEndpoint, azureCredential, databaseName)
    .Options;
//var contextOptions = new DbContextOptionsBuilder<CodeBreakerContext>()
//    .UseCosmos(cosmosConnectionString, databaseName).Options;

var nullLogger = NullLogger<CodeBreakerContext>.Instance;
using var context = new CodeBreakerContext(contextOptions, nullLogger);

Game game = new() {
    GameId = Guid.NewGuid(),
    Code = new string[] { Red, Blue, Black, White },
    Username = "Christian",
    Start = DateTime.Now,
    Colors = Array.Empty<string>(),
    MaxMoves = 12,
};

Console.WriteLine($"Creating game: {game}");

context.Games.Add(game);
await context.SaveChangesAsync();

Console.WriteLine("Created game");

using MemoryGame.Backend.GameLogic;
using MemoryGame.Backend.Api;
using MemoryGame.Backend.DataAccess.InMemory;
using MemoryGame.Backend.DataAccess.Interfaces;
using MemoryGame.Backend.utilities;

// Application entry point (top-level statements, so no type to attach a /// <summary> to):
// registers services, configures CORS and OpenAPI, and maps the game and score endpoints.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IGameManager, GameManager>();
builder.Services.AddSingleton<IScoreRepository, InMemoryScoreRepository>();
builder.Services.AddSingleton<ShuffleHelper>();
builder.Services.AddSingleton<BoardFactory>();
    
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => 
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();
app.MapOpenApi();

app.MapGameEndpoints();
app.MapScoreEndpoints();

app.Run();
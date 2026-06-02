using MemoryGame.Backend.GameLogic;
using MemoryGame.Backend.dataAccess;
using MemoryGame.Backend.Api;
using MemoryGame.Backend.utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IGameManager, GameManager>();
builder.Services.AddSingleton<IScoreRepository, InMemoryScoreRepository>();
builder.Services.AddSingleton<ShuffleHelper>();
    
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
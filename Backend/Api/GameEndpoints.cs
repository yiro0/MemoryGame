using MemoryGame.Backend.Api.Contracts;
using MemoryGame.Backend.dataAccess;
using MemoryGame.Backend.GameLogic;
using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.Api;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/game");
        
        group.MapPost("/start", (IGameManager gameManager, StartGameRequest request) =>
        {
            var settings = new MemoryGame.Backend.GameLogic.GameSettings(request.Values);
            gameManager.StartNewGame(settings);
            return Results.Ok(gameManager.GetBoard());
        });
        
        // Instead of reveal card flip card is being used 
        group.MapPost("/reveal", (RevealRequest request, IGameManager gameManager) =>
        {
            var board = gameManager.FlipCard(request.CardId);
            return Results.Ok(board);
        });
        
        group.MapGet("/state", (IGameManager gameManager) =>
            Results.Ok(gameManager.GetBoard()));
        
        group.MapPost("/score", (ScoreSubmission submission, IScoreRepository scores) =>
        {
            scores.SaveScore(new Score
            {
                PlayerName = submission.PlayerName,
                Moves = submission.Moves,
                TimeSeconds = submission.TimeSeconds,
                CreatedAt = DateTime.UtcNow
            });
            return Results.Created();
        });
    }
    
    public record RevealRequest(int CardId);
    public record ScoreSubmission(string PlayerName, int Moves, int TimeSeconds);
}
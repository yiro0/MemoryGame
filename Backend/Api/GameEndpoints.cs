using MemoryGame.Backend.Api.Contracts;
using MemoryGame.Backend.GameLogic;

namespace MemoryGame.Backend.Api;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/game");
        
        group.MapPost("/start", (IGameManager gameManager, StartGameRequest settings) =>
        {
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
    }

    public record RevealRequest(int CardId);
}
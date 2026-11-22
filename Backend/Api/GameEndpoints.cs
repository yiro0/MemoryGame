using MemoryGame.Backend.Api.Contracts;
using MemoryGame.Backend.GameLogic;

namespace MemoryGame.Backend.Api;

/// <summary>
/// Minimal API endpoints for playing a game, grouped under <c>/game</c>.
/// </summary>
public static class GameEndpoints
{
    /// <summary>
    /// Maps <c>POST /game/start</c>, <c>POST /game/reveal</c> and <c>GET /game/state</c>.
    /// Each returns the current <see cref="GameBoard"/>.
    /// </summary>
    /// <param name="app">The application to register the endpoints on.</param>
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

    /// <summary>
    /// Request body of <c>POST /game/reveal</c>.
    /// </summary>
    /// <param name="CardId">Id of the card to reveal.</param>
    public record RevealRequest(int CardId);
}

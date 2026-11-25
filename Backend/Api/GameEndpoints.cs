using MemoryGame.Backend.Api.Contracts;
using MemoryGame.Backend.DataAccess.Interfaces;
using MemoryGame.Backend.GameLogic;
using MemoryGame.Backend.Models;

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

        // Score saving moves into GameService in step 4.
        group.MapPost("/reveal", (RevealRequest request, IGameManager gameManager, IScoreRepository scoreRepo) =>
        {
            var result = gameManager.RevealCard(request.CardId);
            if (result.JustWon)
            {
                var playerName = gameManager.Settings?.PlayerName;
                scoreRepo.SaveScore(new Score
                {
                    // TODO: Login feature and login credentials
                    PlayerName = string.IsNullOrWhiteSpace(playerName) ? "Anonymous" : playerName,
                    Moves = gameManager.Moves,
                    TimeSeconds = gameManager.ElapsedSeconds,
                });
            }

            return Results.Ok(result.Board);
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

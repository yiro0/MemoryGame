using MemoryGame.Backend.DataAccess.Interfaces;

namespace MemoryGame.Backend.Api;

/// <summary>
/// Minimal API endpoints for reading scores.
/// </summary>
public static class ScoreEndpoints
{
    /// <summary>
    /// Maps <c>GET /score/top?limit=N</c>: the best scores, ordered by fewest moves, then shortest time.
    /// Returns 400 when <c>limit</c> is not positive.
    /// </summary>
    /// <param name="app">The application to register the endpoints on.</param>
    public static void MapScoreEndpoints(this WebApplication app)
    {
        app.MapGet("/score/top", (IScoreRepository scores, int limit = 10) =>
        {
            if (limit <= 0) return Results.BadRequest("Limit must be > 0");
            var top = scores.GetAllScores()
                .OrderBy(s => s.Moves)
                .ThenBy(s => s.TimeSeconds)
                .Take(limit);

            return Results.Ok(top);
        });
    }
}

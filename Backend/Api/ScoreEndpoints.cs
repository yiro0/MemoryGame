using MemoryGame.Backend.DataAccess.Interfaces;

namespace MemoryGame.Backend.Api;

public static class ScoreEndpoints
{
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
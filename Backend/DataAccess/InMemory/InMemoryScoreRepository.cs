using System.Collections.Concurrent;
using MemoryGame.Backend.DataAccess.Interfaces;
using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.DataAccess.InMemory;

/// <summary>
/// Thread-safe, in-memory <see cref="IScoreRepository"/> for development and tests.
/// Scores are lost when the application stops.
/// </summary>
public class InMemoryScoreRepository : IScoreRepository
{
    /// <summary> Largest number of scores one call can return. <c>100</c>.</summary>
    public const int MaxScoresPerCall = 100;
    
    private readonly ConcurrentBag<Score> _scores = new();

    /// <inheritdoc />
    public void SaveScore(Score score)
    {
        _scores.Add(score);
    }

    /// <inheritdoc />
    public IEnumerable<Score> GetTopScores(int limit)
    {
        if (limit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be a positive integer.");
        }
        
        limit = Math.Min(limit, MaxScoresPerCall);
        
        return _scores
            .OrderBy(s => s.Moves)
            .ThenBy(s => s.TimeSeconds)
            .Take(limit)
            .ToArray();
    }
}

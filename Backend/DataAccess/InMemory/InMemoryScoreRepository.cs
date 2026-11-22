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
    private readonly ConcurrentBag<Score> _scores = new();

    /// <inheritdoc />
    public void SaveScore(Score score)
    {
        _scores.Add(score);
    }

    /// <inheritdoc />
    public IEnumerable<Score> GetAllScores() => _scores.ToArray();

    /// <inheritdoc />
    /// <remarks>Not implemented yet: ignores <paramref name="limit"/> and returns every score unsorted.</remarks>
    public IEnumerable<Score> GetTopScores(int limit)
    {
        return _scores;
    }
}

namespace MemoryGame.Backend.dataAccess;

using MemoryGame.Backend.models;
using System.Collections.Concurrent;

public class InMemoryScoreRepository : IScoreRepository 
{
    private readonly ConcurrentBag<Score> _scores = new();

    public void SaveScore(Score score)
    {
        _scores.Add(score);
    }

    public IEnumerable<Score> GetAllScores() => _scores.ToArray();
}
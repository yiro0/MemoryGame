namespace MemoryGame.Backend.dataAccess;

using Models;
using System.Collections.Concurrent;

//In memory implementation of IScoreRepository used for dev/test 
public class InMemoryScoreRepository : IScoreRepository 
{
    private readonly ConcurrentBag<Score> _scores = new();

    public void SaveScore(Score score)
    {
        _scores.Add(score);
    }

    public IEnumerable<Score> GetAllScores() => _scores.ToArray();

    public IEnumerable<Score> GetTopScores(int limit)
    {
        return _scores;
    }
}
namespace MemoryGame.Backend.dataAccess;

using MemoryGame.Backend.models;

public class IScoreRepository
{
    void SaveScore(Score score);
    IEnumerable<Score> GetAllScores();
}
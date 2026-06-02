namespace MemoryGame.Backend.dataAccess;

using Models;

public interface IScoreRepository
{
    //TODO Implement methods for saving and retrieving scores
    //Idk if there will be mutliple implementations of saving scores 
    //interface might be overkill
    //unless there will be multiple ways of saving scores (e.g., in-memory, database, file)
    void SaveScore(Score score);
    IEnumerable<Score> GetAllScores();
    IEnumerable<Score> GetTopScores(int limit);
}
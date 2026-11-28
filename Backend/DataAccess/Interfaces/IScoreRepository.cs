using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.DataAccess.Interfaces;

/// <summary>
/// Storage for finished-game scores. Abstracted so the in-memory store can later be
/// swapped for a database (roadmap Phase 2.4: SQLite / PostgreSQL / LiteDB).
/// </summary>
public interface IScoreRepository
{
    //TODO Implement methods for saving and retrieving scores
    //Idk if there will be mutliple implementations of saving scores
    //interface might be overkill
    //unless there will be multiple ways of saving scores (e.g., in-memory, database, file)

    /// <summary>
    /// Stores a score.
    /// </summary>
    /// <param name="score">The score to store.</param>
    void SaveScore(Score score);

    /// <summary>
    /// Returns the best scores, best first: fewest moves, then shortest time.
    /// </summary>
    /// <param name="limit">How many scores to return. Implementations may cap it.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="limit"/> is zero or negative.</exception>
    IEnumerable<Score> GetTopScores(int limit);
}

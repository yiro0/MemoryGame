namespace MemoryGame.Backend.Models;

/// <summary>
/// Result of a finished game, as stored in the leaderboard.
/// </summary>
public class Score
{
    /// <summary>
    /// Storage id (not assigned by the in-memory repository yet).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the player who finished the game.
    /// </summary>
    public string PlayerName { get; set; } = string.Empty;

    /// <summary>
    /// Number of moves needed to finish (lower is better).
    /// </summary>
    public int Moves { get; set; }

    /// <summary>
    /// Seconds from game start to the last match (lower is better).
    /// </summary>
    public int TimeSeconds { get; set; }

    /// <summary>
    /// When the score was recorded (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

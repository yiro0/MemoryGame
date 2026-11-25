namespace MemoryGame.Backend.GameLogic;

/// <summary>
/// Lifecycle of the active game.
/// </summary>
public enum GameStatus
{
    /// <summary>No game has been started yet.</summary>
    NotStarted,

    /// <summary>A game is being played.</summary>
    InProgress,

    /// <summary>Every card has been matched.</summary>
    Won
}

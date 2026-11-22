namespace MemoryGame.Backend.GameLogic;

/// <summary>
/// Domain-side configuration for a new game, consumed by <see cref="BoardFactory"/>.
/// </summary>
public class GameSettings
{
    /// <summary>
    /// Difficulty name (<c>easy</c>, <c>medium</c>, <c>hard</c>); unknown values fall back to medium.
    /// </summary>
    public string? Difficulty { get; init; }

    /// <summary>
    /// Explicit number of pairs; overrides <see cref="Difficulty"/> when in range.
    /// </summary>
    public int? PairCount { get; init; }

    /// <summary>
    /// Custom card values; topped up from the built-in symbol pool if too few.
    /// </summary>
    public IReadOnlyList<string>? Values { get; init; }

    /// <summary>
    /// Time limit for the game (not enforced yet).
    /// </summary>
    public int? TimeLimitSeconds { get; init; }

    /// <summary>
    /// Name stored with the score (not used yet).
    /// </summary>
    public string? PlayerName { get; init; }

    /// <summary>
    /// Creates settings for the given difficulty.
    /// </summary>
    /// <param name="difficulty">Difficulty name; defaults to <c>medium</c>.</param>
    public GameSettings(string difficulty = "medium")
    {
        Difficulty = difficulty;
    }
}

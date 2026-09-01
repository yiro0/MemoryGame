namespace MemoryGame.Backend.GameLogic;

public class GameSettings
{
    public string? Difficulty { get; init; }
    public int? PairCount { get; init; }
    public IReadOnlyList<string>? Values { get; init; }
    public int? TimeLimitSeconds { get; init; }
    public string? PlayerName { get; init; }

    public GameSettings(string difficulty = "medium")
    {
        Difficulty = difficulty;
    }

    public GameSettings(IEnumerable<string> values)
    {
        Values = values.ToList().AsReadOnly();
    }
}
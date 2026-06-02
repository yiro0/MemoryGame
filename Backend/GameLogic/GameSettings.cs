namespace MemoryGame.Backend.GameLogic;

public class GameSettings
{
    public IReadOnlyList<string> Values { get; init; }
    
    public GameSettings(IEnumerable<string> values)
    {
        Values = values.ToList().AsReadOnly();
    }
    // TODO: Add helpers like diff,time,playername etc.
    // Maybe this should even be interface?
}
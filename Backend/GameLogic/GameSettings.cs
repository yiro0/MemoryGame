namespace MemoryGame.Backend.gameLogic;

public class GameSettings
{
    public IReadOnlyList<string> Values { get; init; }
    
    public GameSettings(IEnumerable<string> values)
    {
        Values = Values.ToList().AsReadOnly();
    }
    // TODO: Add helpers like diff,time,playername etc.
}
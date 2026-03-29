namespace MemoryGame.Backend.dataAccess;

public class GameState
{
    public List<CardDto> Cards { get; set; } = new();
    public int MoveCount { get; set; }
    public bool IsComplete { get; set; }
    public int ElapsedSeconds { get; set; }
}
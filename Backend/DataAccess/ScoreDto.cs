namespace MemoryGame.Backend.dataAccess;

public class ScoreDto
{
    public string PlayerName { get; set; } = string.Empty;
    public int Moves { get; set; }
    public int TimeSeconds { get; set; }
}
namespace MemoryGame.Backend.models;

public class Score
{
    public int Id { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int Moves { get; set; }
    public int TimeSeconds { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
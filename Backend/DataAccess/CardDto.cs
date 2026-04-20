namespace MemoryGame.Backend.dataAccess;

public class CardDto {
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public bool IsFlipped { get; set; }
    public bool IsMatched { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
}
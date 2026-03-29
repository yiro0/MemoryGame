using MemoryGame.Backend.models;

namespace MemoryGame.Backend.gameLogic;

public class Card
{
    public int Id { get; }
    public string Value { get; }
    public bool IsFlipped { get; }
    public bool IsMatched { get; }
    public Position Position { get; }

    public Card(int id, string value, Position position, bool isFlipped = false, bool isMatched = false)
    {
        Id = id; 
        Value = value;
        Position = position;
        IsFlipped = isFlipped;
        IsMatched = isMatched;
    }
    
    public Card Flip() => new Card(Id, Value, Position, !IsFlipped, IsMatched); 
    public Card Match() => new Card(Id, Value, Position, IsFlipped, true);
}
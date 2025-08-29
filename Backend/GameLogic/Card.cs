namespace MemoryGame.Backend.gameLogic;

public class Card
{
    public int Id { get; }
    public string Value { get; }
    public bool IsFlipped { get; }
    public bool IsMatched { get; }

    public Card(int id, string value, bool isFlippped = false, bool isMatched = false)
    {
        Id = id; 
        Value = value;
        IsFlipped = isFlippped;
        IsMatched = isMatched;
    }
    
    public Card Flip() => new Card(Id, Value, !IsFlipped, IsMatched); 
    public Card Match() => new Card(Id, Value, IsFlipped, true);
}
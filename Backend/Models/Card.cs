using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.Models;

/// <summary>
/// A single, immutable card on the board. State changes return a new <see cref="Card"/>.
/// </summary>
public class Card
{
    /// <summary>
    /// Unique id of the card within its board.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Symbol shown on the card's face; the two cards of a pair share it.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Whether the card's face is currently showing.
    /// </summary>
    public bool IsFlipped { get; }

    /// <summary>
    /// Whether the card has been matched with its pair.
    /// </summary>
    public bool IsMatched { get; }

    /// <summary>
    /// Location of the card in the board grid.
    /// </summary>
    public Position Position { get; }

    /// <summary>
    /// Creates a card.
    /// </summary>
    /// <param name="id">Unique id within the board.</param>
    /// <param name="value">Symbol on the card's face.</param>
    /// <param name="position">Location in the grid.</param>
    /// <param name="isFlipped">Whether the face is showing.</param>
    /// <param name="isMatched">Whether the card is already matched.</param>
    public Card(int id, string value, Position position, bool isFlipped = false, bool isMatched = false)
    {
        Id = id;
        Value = value;
        Position = position;
        IsFlipped = isFlipped;
        IsMatched = isMatched;
    }

    /// <summary>
    /// Returns a copy with <see cref="IsFlipped"/> toggled.
    /// </summary>
    public Card Flip() => new Card(Id, Value, Position, !IsFlipped, IsMatched);

    /// <summary>
    /// Returns a copy marked as matched.
    /// </summary>
    public Card Match() => new Card(Id, Value, Position, IsFlipped, true);
}

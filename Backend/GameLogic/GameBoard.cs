using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.GameLogic;

/// <summary>
/// Immutable snapshot of the cards on the table and the move count.
/// Every change returns a new <see cref="GameBoard"/>; game rules live in <see cref="GameManager"/>.
/// </summary>
public class GameBoard
{
    /// <summary>
    /// All cards on the board, in layout order.
    /// </summary>
    public IReadOnlyList<Card> Cards { get; }

    /// <summary>
    /// Total number of cards on the board.
    /// </summary>
    public int BoardSize => Cards.Count;

    /// <summary>
    /// Number of moves made so far (one move = two cards revealed).
    /// </summary>
    public int Moves { get; }

    /// <summary>
    /// Creates a board from the given cards.
    /// </summary>
    /// <param name="cards">Cards to place on the board.</param>
    /// <param name="moves">Moves made so far.</param>
    public GameBoard(IEnumerable<Card> cards, int moves = 0)
    {
        Cards = cards.ToList().AsReadOnly();
        Moves = moves;
    }

    /// <summary>
    /// Returns a new board with the face of the given card toggled.
    /// </summary>
    /// <param name="cardId">Id of the card to toggle.</param>
    public GameBoard FlipCard(int cardId)
    {
        var updateCards = Cards
            .Select(card => card.Id == cardId ? card.Flip() : card)
            .ToList();
        return new GameBoard(updateCards, Moves);
    }

    /// <summary>
    /// Returns a new board with the given cards marked as matched.
    /// </summary>
    /// <param name="cardIds">Ids of the cards that form a matched pair.</param>
    public GameBoard MatchCards(IEnumerable<int> cardIds)
    {
        var idSet = new HashSet<int>(cardIds);
        var updateCards = Cards
            .Select(card => idSet.Contains(card.Id) ? card.Match() : card)
            .ToList();
        return new GameBoard(updateCards, Moves);
    }
}

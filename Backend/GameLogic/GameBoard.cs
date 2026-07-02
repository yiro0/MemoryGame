using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.GameLogic;

public class GameBoard
{
    public IReadOnlyList<Card> Cards { get; }
    public int BoardSize => Cards.Count;
    public int Moves { get; }

    public GameBoard(IEnumerable<Card> cards, int moves = 0)
    {
        Cards = cards.ToList().AsReadOnly();
        Moves = moves;
    }
    
    public GameBoard FlipCard(int cardId)
    {
        var updateCards = Cards
            .Select(card => card.Id == cardId ? card.Flip() : card)
            .ToList();
        return new GameBoard(updateCards, Moves);
    }

    public GameBoard MatchCards(IEnumerable<int> cardIds)
    {
        var idSet = new HashSet<int>(cardIds);
        var updateCards = Cards
            .Select(card => idSet.Contains(card.Id) ? card.Match() : card)
            .ToList();
        return new GameBoard(updateCards, Moves);
    }
}
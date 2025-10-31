namespace MemoryGame.Backend.gameLogic;

public class GameBoard
{
    public IReadOnlyList<Card> Cards { get; }

    public GameBoard(IEnumerable<Card> cards)
    {
        Cards = cards.ToList().AsReadOnly();
    }
    
    public GameBoard FlipCard(int cardId)
    {
        var updateCards = Cards
            .Select(card => card.Id == cardId ? card.Flip() : card)
            .ToList();
        return new GameBoard(updateCards);
    }

    public GameBoard MatchCards(IEnumerable<int> cardIds)
    {
        var idSet = new HashSet<int>(cardIds);
        var updateCards = Cards
            .Select(card => idSet.Contains(card.Id) ? card.Match() : card)
            .ToList();
        return new GameBoard(updateCards);
    }
}
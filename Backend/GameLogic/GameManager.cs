using MemoryGame.Backend.utilities;

namespace MemoryGame.Backend.gameLogic;

public class GameManager : IGameManager
{
    private readonly IScoreRepository _scoreRepo;
    private readonly ShuffleHelper _shuffle;
    private GameBoard _board;

    public GameManager(IScoreRepository scoreRepo, ShuffleHelper shuffle)
    {
        _scoreRepo = scoreRepo;
        _shuffle = shuffle;
        _board = new GameBoard(new List<Card>());
    }
    
    public GameBoard GetBoard() => _board;
    
    public void StartNewGame(GameSettings settings)
    {
        var values = settings.Values;
        var cards = values
            .SelectMany((v, idx) => new[]
        {
            new Card(idx * 2 + 1, v),
            new Card(idx * 2 + 2, v)
        })
        .ToList();
        
        _shuffle.Shuffle(cards);
        _board = new GameBoard(cards);
    }

    public GameBoard FlipCard(int cardId)
    {
        _board = _board.FlipCard(cardId);
        var flipped = _board.Cards.Where(c => c.IsFlipped && !c.IsMatched).ToList();
        if (flipped.Count == 2 && flipped[0].Value == flipped[1].Value)
        {
            _board = _board.MatchCards(flipped.Select(c => c.Id));
        }
        
        if (IsComplete())
        {
            _scoreRepo?.SaveScore(new models.Score
            {
                /* Fill the fields */
            });
        }

        return _board;
    }
    public bool IsComplete() => _board.Cards.All(c => c.IsMatched);
}
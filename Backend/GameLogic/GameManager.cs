using MemoryGame.Backend.utilities;
using MemoryGame.Backend.dataAccess;
using MemoryGame.Backend.Models;

namespace MemoryGame.Backend.GameLogic;

public class GameManager : IGameManager
{
    private readonly IScoreRepository _scoreRepo;
    private readonly ShuffleHelper _shuffle;
    private GameBoard _board;
    private DateTime _gameStartTime;
    private int _moveCount;

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
            new Card(idx * 2 + 1, v, new Position(0, 0)),
            new Card(idx * 2 + 2, v, new Position(0, 0))
        })
        .ToList();
        
        _shuffle.Shuffle(cards);
        
        // Assign positions based on grid layout after shuffling
        var gridSize = (int)Math.Ceiling(Math.Sqrt(cards.Count));
        var cardsWithPositions = cards
            .Select((card, index) => 
            {
                var row = index / gridSize;
                var col = index % gridSize;
                return new Card(card.Id, card.Value, new Position(row, col), card.IsFlipped, card.IsMatched);
            })
            .ToList();
        
        _board = new GameBoard(cardsWithPositions);
        _gameStartTime = DateTime.UtcNow;
        _moveCount = 0;
    }

    public GameBoard FlipCard(int cardId)
    {
        _board = _board.FlipCard(cardId);
        var flipped = _board.Cards.Where(c => c.IsFlipped && !c.IsMatched).ToList();
        if (flipped.Count ==2)
        {
            _moveCount++;
            if (flipped[0].Value == flipped[1].Value)
            {
                _board = _board.MatchCards(flipped.Select(c => c.Id));
            }
        }
        
        if (IsComplete())
        {
            _scoreRepo?.SaveScore(new Models.Score
            {
                // TODO: Edit once front hook is ready 
                // This should be fetched from front
                PlayerName = "Yerho",
                Moves = _moveCount,
                TimeSeconds = (int)(DateTime.UtcNow - _gameStartTime).TotalSeconds,
            });
        }

        return _board;
    }
    
    public bool IsComplete() => _board.Cards.All(c => c.IsMatched);
}
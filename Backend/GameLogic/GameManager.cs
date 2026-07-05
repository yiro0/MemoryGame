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
    
    // Keep this logic once frontend is updated to use GameSettings
    public void StartNewGame(string difficulty)
    {
        var symbolPool = new[]
        {
            "dog", "cat", "fox", "frog",
            "panda", "lion", "tiger", "horse",
            "koala", "butterfly", "snake", "bird",
            "turtle", "cow", "pig", "rabbit",
            "monkey", "elephant", "penguin", "bear",
            "duck", "sheep"
        };

        var difficultyMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "easy", 6 },
            { "medium", 8 },
            { "hard", 10 }
        };

        var pairs = difficultyMap.ContainsKey(difficulty)
            ? difficultyMap[difficulty]
            : difficultyMap["medium"];

        var rnd = new Random();
        var shuffledPool = symbolPool.OrderBy(eachSymbol => rnd.Next()).ToList();
        var chosen = shuffledPool.Take(pairs).ToList();

        var values = chosen;

        var cards = values
            .SelectMany((v, idx) => new[]
        {
            new Card(idx * 2 + 1, v, new Position(0, 0)),
            new Card(idx * 2 + 2, v, new Position(0, 0))
        })
        .ToList();

        _shuffle.Shuffle(cards);

        var gridSize = (int)Math.Ceiling(Math.Sqrt(cards.Count));
        var cardsWithPositions = cards
            .Select((card, index) => 
            {
                var row = index / gridSize;
                var col = index % gridSize;
                return new Card(card.Id, card.Value, new Position(row, col), card.IsFlipped, card.IsMatched);
            })
            .ToList();

        _moveCount = 0;
        _board = new GameBoard(cardsWithPositions, _moveCount);
        _gameStartTime = DateTime.UtcNow;
    }

    public void StartNewGame(GameSettings settings)
    {
        if (settings?.Values != null && settings.Values.Count > 0)
        {
            // this will change when 
            // difficulty-based part of backend will be properly implemented 
            var single = settings.Values.Count == 1
                ? settings.Values[0]
                : null;
            var difficultyMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "easy", 6 },
                { "medium", 8 },
                { "hard", 10 }
            };

            if (single != null && difficultyMap.ContainsKey(single))
            {
                StartNewGame(single);
                return;
            }
            
            var values = settings.Values;
            var cards = values
                .SelectMany((v, idx) => new[]
            {
                new Card(idx * 2 + 1, v, new Position(0, 0)),
                new Card(idx * 2 + 2, v, new Position(0, 0))
            })
            .ToList();

            _shuffle.Shuffle(cards);
            var gridSize = (int)Math.Ceiling(Math.Sqrt(cards.Count));
            var cardsWithPositions = cards
                .Select((card, index) =>
                {
                    var row = index / gridSize;
                    var col = index % gridSize;
                    return new Card(card.Id, card.Value, new Position(row, col), card.IsFlipped, card.IsMatched);
                })
                .ToList();

            _moveCount = 0;
            _board = new GameBoard(cardsWithPositions, _moveCount);
            _gameStartTime = DateTime.UtcNow;
            return;
        }

        // Fallback to default difficulty-based start
        StartNewGame("medium");
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
                // TODO: Login feature and login credentials
                PlayerName = "Yerho",
                Moves = _moveCount,
                TimeSeconds = (int)(DateTime.UtcNow - _gameStartTime).TotalSeconds,
            });
        }

        _board = new GameBoard(_board.Cards, _moveCount);
        
        return _board;
    }
    
    public bool IsComplete() => _board.Cards.All(c => c.IsMatched);
}
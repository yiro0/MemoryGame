using MemoryGame.Backend.DataAccess.Interfaces;
using MemoryGame.Backend.utilities;
using MemoryGame.Backend.Models;
using MemoryGame.Backend.Api.Contracts;

namespace MemoryGame.Backend.GameLogic;

public class GameManager : IGameManager
{
    private static readonly string[] _symbolPool =
    {
        "dog", "cat", "fox", "frog",
        "panda", "lion", "tiger", "horse",
        "koala", "butterfly", "snake", "bird",
        "turtle", "cow", "pig", "rabbit",
        "monkey", "elephant", "penguin", "bear",
        "duck", "sheep"
    };

    private static readonly Dictionary<string, int> _difficultyMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "easy", 6 },
            { "medium", 8 },
            { "hard", 10 }
        };

    private const string DefaultDifficulty = "medium";

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
    
    public void StartNewGame(string difficulty)
    {
        StartNewGame(new GameSettings(difficulty));
    }

    public void StartNewGame(GameSettings settings)
    {
        var effectiveDifficulty = ResolveDifficulty(settings?.Difficulty);
        var pairCount = ResolvePairCount(settings?.PairCount, effectiveDifficulty);

        var customValues = settings?.Values?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        List<string> values;
        if (customValues != null && customValues.Count > 0)
        {
            values = customValues.Take(pairCount).ToList();

            if (values.Count < pairCount)
            {
                var remaining = _symbolPool
                    .Where(symbol => values.All(existing =>
                        !string.Equals(existing, symbol, StringComparison.OrdinalIgnoreCase)))
                    .Take(pairCount - values.Count);

                values.AddRange(remaining);
            }
        }
        else
        {
            var rnd = new Random();
            values = _symbolPool
                .OrderBy(_ => rnd.Next())
                .Take(pairCount)
                .ToList();
        }

        if (values.Count < pairCount)
        {
            pairCount = _difficultyMap[DefaultDifficulty];
            var rnd = new Random();
            values = _symbolPool
                .OrderBy(_ => rnd.Next())
                .Take(pairCount)
                .ToList();
        }

        var cards = values
            .SelectMany((value, idx) => new[]
            {
                new Card(idx * 2 + 1, value, new Position(0, 0)),
                new Card(idx * 2 + 2, value, new Position(0, 0))
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

    public void StartNewGame(StartGameRequest settings)
    {
        var requestSettings = settings?.Settings;
        var mappedSettings = new GameSettings(
            requestSettings?.Difficulty ?? settings?.Difficulty ?? DefaultDifficulty)
        {
            PairCount = requestSettings?.PairCount,
            Values = requestSettings?.Values,
            TimeLimitSeconds = requestSettings?.TimeLimitSeconds,
            PlayerName = requestSettings?.PlayerName
        };

        StartNewGame(mappedSettings);
    }

    private static string ResolveDifficulty(string? difficulty) =>
        string.IsNullOrWhiteSpace(difficulty) || !_difficultyMap.ContainsKey(difficulty)
            ? DefaultDifficulty
            : difficulty;

    private static int ResolvePairCount(int? pairCount, string difficulty)
    {
        if (pairCount is > 0 && pairCount <= _symbolPool.Length)
        {
            return pairCount.Value;
        }

        return _difficultyMap[difficulty];
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
using MemoryGame.Backend.Models;
using MemoryGame.Backend.utilities;

namespace MemoryGame.Backend.GameLogic;

/// <summary>
/// Builds a fresh, shuffled <see cref="GameBoard"/> from <see cref="GameSettings"/>.
/// Stateless: owns the symbol pool, difficulty rules and card layout, but never game progress.
/// </summary>
public class BoardFactory
{
    private static readonly string[] SymbolPool =
    {
        "dog", "cat", "fox", "frog",
        "panda", "lion", "tiger", "horse",
        "koala", "butterfly", "snake", "bird",
        "turtle", "cow", "pig", "rabbit",
        "monkey", "elephant", "penguin", "bear",
        "duck", "sheep"
    };

    private static readonly Dictionary<string, int> DifficultyPairs =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "easy", 6 },
            { "medium", 8 },
            { "hard", 10 }
        };

    public const string DefaultDifficulty = "medium";

    private readonly ShuffleHelper _shuffle;

    public BoardFactory(ShuffleHelper shuffle)
    {
        _shuffle = shuffle;
    }

    /// <param name="seed">Optional seed for reproducible boards (roadmap Phase 6 determinism).</param>
    public GameBoard Create(GameSettings? settings, int? seed = null)
    {
        var difficulty = ResolveDifficulty(settings?.Difficulty);
        var pairCount = ResolvePairCount(settings?.PairCount, difficulty);
        var values = PickValues(settings?.Values, pairCount, seed);

        var cards = values
            .SelectMany((value, idx) => new[]
            {
                new Card(idx * 2 + 1, value, new Position(0, 0)),
                new Card(idx * 2 + 2, value, new Position(0, 0))
            })
            .ToList();

        _shuffle.Shuffle(cards, seed);

        return new GameBoard(AssignPositions(cards));
    }

    private static string ResolveDifficulty(string? difficulty) =>
        string.IsNullOrWhiteSpace(difficulty) || !DifficultyPairs.ContainsKey(difficulty)
            ? DefaultDifficulty
            : difficulty;

    private static int ResolvePairCount(int? pairCount, string difficulty) =>
        pairCount is > 0 && pairCount <= SymbolPool.Length
            ? pairCount.Value
            : DifficultyPairs[difficulty];

    /// <summary>
    /// Custom values first (distinct, non-blank), topped up from the pool.
    /// Without custom values, a random subset of the pool.
    /// </summary>
    private List<string> PickValues(IReadOnlyList<string>? customValues, int pairCount, int? seed)
    {
        var values = (customValues ?? [])
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(pairCount)
            .ToList();

        var pool = SymbolPool
            .Where(symbol => !values.Contains(symbol, StringComparer.OrdinalIgnoreCase))
            .ToList();
        _shuffle.Shuffle(pool, seed);

        values.AddRange(pool.Take(pairCount - values.Count));
        return values;
    }

    private static List<Card> AssignPositions(List<Card> cards)
    {
        var gridSize = (int)Math.Ceiling(Math.Sqrt(cards.Count));
        return cards
            .Select((card, index) => new Card(
                card.Id, card.Value, new Position(index / gridSize, index % gridSize),
                card.IsFlipped, card.IsMatched))
            .ToList();
    }
}

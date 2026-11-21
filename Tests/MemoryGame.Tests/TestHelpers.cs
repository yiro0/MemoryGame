using MemoryGame.Backend.DataAccess.InMemory;
using MemoryGame.Backend.GameLogic;
using MemoryGame.Backend.Models;
using MemoryGame.Backend.utilities;

namespace MemoryGame.Tests;

internal static class TestHelpers
{
    public static (GameManager Manager, InMemoryScoreRepository Scores) CreateManager()
    {
        var scores = new InMemoryScoreRepository();
        return (new GameManager(scores, CreateFactory()), scores);
    }

    public static BoardFactory CreateFactory() => new(new ShuffleHelper());

    /// <summary>Returns the ids of every pair on the board, grouped by card value.</summary>
    public static List<(int First, int Second)> Pairs(GameBoard board) =>
        board.Cards
            .GroupBy(c => c.Value)
            .Select(g => (g.ElementAt(0).Id, g.ElementAt(1).Id))
            .ToList();

    /// <summary>Two card ids whose values differ.</summary>
    public static (int First, int Second) Mismatch(GameBoard board)
    {
        var first = board.Cards[0];
        var second = board.Cards.First(c => c.Value != first.Value);
        return (first.Id, second.Id);
    }

    public static Card CardById(GameBoard board, int id) => board.Cards.Single(c => c.Id == id);
}

using MemoryGame.Backend.Api.Contracts;
using MemoryGame.Backend.GameLogic;
using static MemoryGame.Tests.TestHelpers;

namespace MemoryGame.Tests;

public class BoardCreationTests
{
    [Theory]
    [InlineData("easy", 12)]
    [InlineData("medium", 16)]
    [InlineData("hard", 20)]
    [InlineData("HARD", 20)]
    public void Difficulty_DeterminesCardCount(string difficulty, int expectedCards)
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new GameSettings(difficulty));

        Assert.Equal(expectedCards, manager.GetBoard().Cards.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("impossible")]
    public void UnknownDifficulty_FallsBackToMedium(string? difficulty)
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new StartGameRequest(Difficulty: difficulty));

        Assert.Equal(16, manager.GetBoard().Cards.Count);
    }

    [Fact]
    public void EveryValue_AppearsExactlyTwice()
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new GameSettings("hard"));

        Assert.All(manager.GetBoard().Cards.GroupBy(c => c.Value), g => Assert.Equal(2, g.Count()));
    }

    [Fact]
    public void CardIds_AreUnique()
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new GameSettings("hard"));

        var ids = manager.GetBoard().Cards.Select(c => c.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void Positions_AreUniqueAndInsideSquareGrid()
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new GameSettings("easy"));

        var cards = manager.GetBoard().Cards;
        var gridSize = (int)Math.Ceiling(Math.Sqrt(cards.Count));
        Assert.Equal(cards.Count, cards.Select(c => (c.Position.Row, c.Position.Column)).Distinct().Count());
        Assert.All(cards, c =>
        {
            Assert.InRange(c.Position.Row, 0, gridSize - 1);
            Assert.InRange(c.Position.Column, 0, gridSize - 1);
        });
    }

    [Fact]
    public void NewBoard_StartsHiddenWithZeroMoves()
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new GameSettings());

        var board = manager.GetBoard();
        Assert.Equal(0, board.Moves);
        Assert.All(board.Cards, c => Assert.False(c.IsFlipped || c.IsMatched));
    }

    [Fact]
    public void ExplicitPairCount_OverridesDifficulty()
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new GameSettings("easy") { PairCount = 3 });

        Assert.Equal(6, manager.GetBoard().Cards.Count);
    }

    [Fact]
    public void CustomValues_AreUsedAndTopUpFromPool()
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new GameSettings("easy") { PairCount = 4, Values = ["sun", "moon"] });

        var values = manager.GetBoard().Cards.Select(c => c.Value).Distinct().ToList();
        Assert.Equal(4, values.Count);
        Assert.Contains("sun", values);
        Assert.Contains("moon", values);
    }

    [Fact]
    public void StartGameRequest_NestedSettingsWinOverTopLevelDifficulty()
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new StartGameRequest(new GameSettingsRequest(Difficulty: "hard"), Difficulty: "easy"));

        Assert.Equal(20, manager.GetBoard().Cards.Count);
    }
}

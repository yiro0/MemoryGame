using MemoryGame.Backend.Api.Contracts;
using MemoryGame.Backend.GameLogic;
using static MemoryGame.Tests.TestHelpers;

namespace MemoryGame.Tests;

public class BoardFactoryTests
{
    private readonly BoardFactory _factory = CreateFactory();

    [Theory]
    [InlineData("easy", 12)]
    [InlineData("medium", 16)]
    [InlineData("hard", 20)]
    [InlineData("HARD", 20)]
    public void Difficulty_DeterminesCardCount(string difficulty, int expectedCards)
    {
        var board = _factory.Create(new GameSettings(difficulty));

        Assert.Equal(expectedCards, board.Cards.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("impossible")]
    public void UnknownDifficulty_FallsBackToMedium(string? difficulty)
    {
        var board = _factory.Create(new GameSettings { Difficulty = difficulty });

        Assert.Equal(16, board.Cards.Count);
    }

    [Fact]
    public void NullSettings_FallsBackToMedium()
    {
        Assert.Equal(16, _factory.Create(null).Cards.Count);
    }

    [Fact]
    public void EveryValue_AppearsExactlyTwice()
    {
        var board = _factory.Create(new GameSettings("hard"));

        Assert.All(board.Cards.GroupBy(c => c.Value), g => Assert.Equal(2, g.Count()));
    }

    [Fact]
    public void CardIds_AreUnique()
    {
        var ids = _factory.Create(new GameSettings("hard")).Cards.Select(c => c.Id).ToList();

        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void Positions_AreUniqueAndInsideSquareGrid()
    {
        var cards = _factory.Create(new GameSettings("easy")).Cards;

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
        var board = _factory.Create(new GameSettings());

        Assert.Equal(0, board.Moves);
        Assert.All(board.Cards, c => Assert.False(c.IsFlipped || c.IsMatched));
    }

    [Fact]
    public void ExplicitPairCount_OverridesDifficulty()
    {
        var board = _factory.Create(new GameSettings("easy") { PairCount = 3 });

        Assert.Equal(6, board.Cards.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(23)]
    public void OutOfRangePairCount_FallsBackToDifficulty(int pairCount)
    {
        var board = _factory.Create(new GameSettings("easy") { PairCount = pairCount });

        Assert.Equal(12, board.Cards.Count);
    }

    [Fact]
    public void CustomValues_AreUsedAndTopUpFromPool()
    {
        var board = _factory.Create(new GameSettings("easy") { PairCount = 4, Values = ["sun", "moon"] });

        var values = board.Cards.Select(c => c.Value).Distinct().ToList();
        Assert.Equal(4, values.Count);
        Assert.Contains("sun", values);
        Assert.Contains("moon", values);
    }

    [Fact]
    public void CustomValues_IgnoreBlanksAndCaseInsensitiveDuplicates()
    {
        var board = _factory.Create(new GameSettings { PairCount = 2, Values = ["Sun", "sun", " ", "moon"] });

        Assert.Equal(["moon", "Sun"], board.Cards.Select(c => c.Value).Distinct().Order());
    }

    [Fact]
    public void SameSeed_GivesSameBoard()
    {
        var first = _factory.Create(new GameSettings("hard"), seed: 7);
        var second = _factory.Create(new GameSettings("hard"), seed: 7);

        Assert.Equal(
            first.Cards.Select(c => (c.Id, c.Value, c.Position.Row, c.Position.Column)),
            second.Cards.Select(c => (c.Id, c.Value, c.Position.Row, c.Position.Column)));
    }
}

/// <summary>Request-to-settings mapping still lives in GameManager until step 4 moves it to the API layer.</summary>
public class StartGameRequestMappingTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("impossible")]
    public void UnknownDifficulty_FallsBackToMedium(string? difficulty)
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new StartGameRequest(Difficulty: difficulty));

        Assert.Equal(16, manager.GetBoard().Cards.Count);
    }

    [Fact]
    public void NestedSettings_WinOverTopLevelDifficulty()
    {
        var (manager, _) = CreateManager();

        manager.StartNewGame(new StartGameRequest(new GameSettingsRequest(Difficulty: "hard"), Difficulty: "easy"));

        Assert.Equal(20, manager.GetBoard().Cards.Count);
    }
}

using MemoryGame.Backend.GameLogic;
using static MemoryGame.Tests.TestHelpers;

namespace MemoryGame.Tests;

/// <summary>
/// Reveal, matching, move counting and win rules of <see cref="GameManager"/>.
/// </summary>
public class GameplayTests
{
    private const string FixedInStep3 = "Known bug - server does not enforce reveal rules yet (Phase 4, step 3)";

    [Fact]
    public void RevealingMatchingPair_MarksBothMatched_AndCountsOneMove()
    {
        var (manager, _) = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var (a, b) = Pairs(manager.GetBoard())[0];

        manager.FlipCard(a);
        var board = manager.FlipCard(b);

        Assert.True(CardById(board, a).IsMatched);
        Assert.True(CardById(board, b).IsMatched);
        Assert.Equal(1, board.Moves);
    }

    [Fact]
    public void RevealingMismatch_LeavesBothUnmatched_AndCountsOneMove()
    {
        var (manager, _) = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var (a, b) = Mismatch(manager.GetBoard());

        manager.FlipCard(a);
        var board = manager.FlipCard(b);

        Assert.False(CardById(board, a).IsMatched);
        Assert.False(CardById(board, b).IsMatched);
        Assert.Equal(1, board.Moves);
    }

    [Fact]
    public void RevealingFirstCard_DoesNotCountAMove()
    {
        var (manager, _) = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));

        var board = manager.FlipCard(manager.GetBoard().Cards[0].Id);

        Assert.Equal(0, board.Moves);
    }

    [Fact]
    public void MatchingAllPairs_CompletesGame_AndSavesScore()
    {
        var (manager, scores) = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var pairs = Pairs(manager.GetBoard());

        foreach (var (a, b) in pairs)
        {
            Assert.False(manager.IsComplete());
            manager.FlipCard(a);
            manager.FlipCard(b);
        }

        Assert.True(manager.IsComplete());
        var score = Assert.Single(scores.GetAllScores());
        Assert.Equal(pairs.Count, score.Moves);
    }

    [Fact]
    public void StartingNewGame_ResetsMoves()
    {
        var (manager, _) = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var (a, b) = Mismatch(manager.GetBoard());
        manager.FlipCard(a);
        manager.FlipCard(b);

        manager.StartNewGame(new GameSettings("easy"));

        Assert.Equal(0, manager.GetBoard().Moves);
    }

    // ---- Edge cases from roadmap Phase 2.5 - currently failing, enabled in step 3 ----

    [Fact(Skip = FixedInStep3)]
    public void RevealingSameCardTwice_KeepsItRevealed_AndCountsNoMove()
    {
        var (manager, _) = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var id = manager.GetBoard().Cards[0].Id;

        manager.FlipCard(id);
        var board = manager.FlipCard(id);

        Assert.True(CardById(board, id).IsFlipped);
        Assert.Equal(0, board.Moves);
    }

    [Fact(Skip = FixedInStep3)]
    public void RevealingMatchedCard_ChangesNothing()
    {
        var (manager, _) = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var (a, b) = Pairs(manager.GetBoard())[0];
        manager.FlipCard(a);
        manager.FlipCard(b);

        var board = manager.FlipCard(a);

        Assert.True(CardById(board, a).IsFlipped);
        Assert.True(CardById(board, a).IsMatched);
        Assert.Equal(1, board.Moves);
    }

    [Fact(Skip = FixedInStep3)]
    public void RevealAfterMismatch_HidesPreviousPair()
    {
        var (manager, _) = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var board = manager.GetBoard();
        var (a, b) = Mismatch(board);
        var third = board.Cards.First(c => c.Id != a && c.Id != b).Id;
        manager.FlipCard(a);
        manager.FlipCard(b);

        board = manager.FlipCard(third);

        Assert.False(CardById(board, a).IsFlipped);
        Assert.False(CardById(board, b).IsFlipped);
        Assert.True(CardById(board, third).IsFlipped);
    }

    [Fact(Skip = FixedInStep3)]
    public void RevealAfterWin_DoesNotSaveScoreAgain()
    {
        var (manager, scores) = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var pairs = Pairs(manager.GetBoard());
        foreach (var (a, b) in pairs)
        {
            manager.FlipCard(a);
            manager.FlipCard(b);
        }

        manager.FlipCard(pairs[0].First);

        Assert.Single(scores.GetAllScores());
    }

    [Fact(Skip = FixedInStep3)]
    public void RevealBeforeAnyGame_SavesNoScore()
    {
        var (manager, scores) = CreateManager();

        manager.FlipCard(1);

        Assert.Empty(scores.GetAllScores());
    }
}

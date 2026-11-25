using MemoryGame.Backend.GameLogic;
using static MemoryGame.Tests.TestHelpers;

namespace MemoryGame.Tests;

/// <summary>
/// Reveal, matching, move counting and win rules of <see cref="GameManager"/>.
/// </summary>
public class GameplayTests
{
    [Fact]
    public void RevealingMatchingPair_MarksBothMatched_AndCountsOneMove()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var (a, b) = Pairs(manager.GetBoard())[0];

        manager.RevealCard(a);
        var board = manager.RevealCard(b).Board;

        Assert.True(CardById(board, a).IsMatched);
        Assert.True(CardById(board, b).IsMatched);
        Assert.Equal(1, board.Moves);
    }

    [Fact]
    public void RevealingMismatch_LeavesBothUnmatched_AndCountsOneMove()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var (a, b) = Mismatch(manager.GetBoard());

        manager.RevealCard(a);
        var board = manager.RevealCard(b).Board;

        Assert.False(CardById(board, a).IsMatched);
        Assert.False(CardById(board, b).IsMatched);
        Assert.Equal(1, board.Moves);
    }

    [Fact]
    public void RevealingFirstCard_DoesNotCountAMove()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));

        var board = manager.RevealCard(manager.GetBoard().Cards[0].Id).Board;

        Assert.Equal(0, board.Moves);
    }

    [Fact]
    public void MatchingAllPairs_WinsGame_AndReportsItOnLastReveal()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var pairs = Pairs(manager.GetBoard());
        RevealResult? last = null;

        foreach (var (a, b) in pairs)
        {
            Assert.Equal(GameStatus.InProgress, manager.Status);
            Assert.False(last?.JustWon ?? false);
            manager.RevealCard(a);
            last = manager.RevealCard(b);
        }

        Assert.True(last!.JustWon);
        Assert.Equal(GameStatus.Won, manager.Status);
        Assert.Equal(pairs.Count, manager.Moves);
    }

    [Fact]
    public void StartingNewGame_ResetsMoves()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var (a, b) = Mismatch(manager.GetBoard());
        manager.RevealCard(a);
        manager.RevealCard(b);

        manager.StartNewGame(new GameSettings("easy"));

        Assert.Equal(0, manager.GetBoard().Moves);
    }

    // ---- Edge cases from roadmap Phase 2.5 ----

    [Fact]
    public void RevealingSameCardTwice_KeepsItRevealed_AndCountsNoMove()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var id = manager.GetBoard().Cards[0].Id;

        manager.RevealCard(id);
        var board = manager.RevealCard(id).Board;

        Assert.True(CardById(board, id).IsFlipped);
        Assert.Equal(0, board.Moves);
    }

    [Fact]
    public void RevealingMatchedCard_ChangesNothing()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var (a, b) = Pairs(manager.GetBoard())[0];
        manager.RevealCard(a);
        manager.RevealCard(b);

        var board = manager.RevealCard(a).Board;

        Assert.True(CardById(board, a).IsFlipped);
        Assert.True(CardById(board, a).IsMatched);
        Assert.Equal(1, board.Moves);
    }

    [Fact]
    public void RevealAfterMismatch_HidesPreviousPair()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var board = manager.GetBoard();
        var (a, b) = Mismatch(board);
        var third = board.Cards.First(c => c.Id != a && c.Id != b).Id;
        manager.RevealCard(a);
        manager.RevealCard(b);

        board = manager.RevealCard(third).Board;

        Assert.False(CardById(board, a).IsFlipped);
        Assert.False(CardById(board, b).IsFlipped);
        Assert.True(CardById(board, third).IsFlipped);
    }

    [Fact]
    public void InvalidReveal_KeepsPendingMismatchVisible()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var (a, b) = Mismatch(manager.GetBoard());
        manager.RevealCard(a);
        manager.RevealCard(b);

        var board = manager.RevealCard(-1).Board;

        Assert.True(CardById(board, a).IsFlipped);
        Assert.True(CardById(board, b).IsFlipped);
        Assert.Equal(1, board.Moves);
    }

    [Fact]
    public void RevealAfterWin_DoesNotReportWinAgain()
    {
        var manager = CreateManager();
        manager.StartNewGame(new GameSettings("easy"));
        var pairs = Pairs(manager.GetBoard());
        foreach (var (a, b) in pairs)
        {
            manager.RevealCard(a);
            manager.RevealCard(b);
        }

        var result = manager.RevealCard(pairs[0].First);

        Assert.False(result.JustWon);
        Assert.Equal(GameStatus.Won, manager.Status);
    }

    [Fact]
    public void RevealBeforeAnyGame_DoesNothing()
    {
        var manager = CreateManager();

        var result = manager.RevealCard(1);

        Assert.False(result.JustWon);
        Assert.Empty(result.Board.Cards);
        Assert.Equal(GameStatus.NotStarted, manager.Status);
    }
}

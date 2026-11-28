using MemoryGame.Backend.DataAccess.InMemory;
using MemoryGame.Backend.Models;

namespace MemoryGame.Tests;

/// <summary>
/// Storage and ranking behaviour of <see cref="InMemoryScoreRepository"/>.
/// </summary>
public class InMemoryScoreRepositoryTests
{
    [Fact]
    public void SavedScores_AreReturned()
    {
        var repo = new InMemoryScoreRepository();

        repo.SaveScore(new Score { PlayerName = "a", Moves = 5 });
        repo.SaveScore(new Score { PlayerName = "b", Moves = 3 });

        Assert.Equal(2, repo.GetTopScores(10).Count());
    }

    [Fact]
public void TopScores_AreOrderedByFewestMoves()
    {
        var repo = new InMemoryScoreRepository();
        repo.SaveScore(new Score { PlayerName = "slow", Moves = 10 });
        repo.SaveScore(new Score { PlayerName = "fast", Moves = 4 });

        var top = repo.GetTopScores(10).ToList();

        Assert.Equal("fast", top[0].PlayerName);
    }

    [Fact]
    public void TopScores_BreakTiesByTime()
    {
        // Equal moves: lower time wins.
        var repo = new InMemoryScoreRepository();
        repo.SaveScore(new Score { PlayerName = "slow", Moves = 10, TimeSeconds = 100 });
        repo.SaveScore(new Score { PlayerName = "super slow", Moves = 10, TimeSeconds = 200 });

        var top = repo.GetTopScores(2).ToList();

        Assert.Equal("slow", top[0].PlayerName);
    }

    [Fact]
    public void TopScores_ReturnAtMostLimit()
    {
        var repo = new InMemoryScoreRepository();
        repo.SaveScore(new Score { PlayerName = "slow", Moves = 10 });
        repo.SaveScore(new Score { PlayerName = "medium", Moves = 6 });
        repo.SaveScore(new Score { PlayerName = "fast", Moves = 4 });

        var top = repo.GetTopScores(2).ToList();

        Assert.Equal(2, top.Count);
    }

    [Fact]
    public void TopScores_AreCappedAtMaxScoresPerCall()
    {
        var repo = new InMemoryScoreRepository();
        for (var i = 0; i < 150; i++)
            repo.SaveScore(new Score { PlayerName = $"p{i}", Moves = i });

        var top = repo.GetTopScores(1000).ToList();

    Assert.Equal(InMemoryScoreRepository.MaxScoresPerCall, top.Count);
        Assert.Equal(0, top[0].Moves);
        Assert.Equal(99, top[^1].Moves);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void TopScores_NonPositiveLimit_Throws(int limit)
    {
        var repo = new InMemoryScoreRepository();

        Assert.Throws<ArgumentOutOfRangeException>(() => repo.GetTopScores(limit).ToList());
    }
}

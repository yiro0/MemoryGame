using MemoryGame.Backend.DataAccess.InMemory;
using MemoryGame.Backend.Models;
using MemoryGame.Backend.utilities;

namespace MemoryGame.Tests;

/// <summary>
/// Determinism and correctness of <see cref="ShuffleHelper"/>.
/// </summary>
public class ShuffleHelperTests
{
    [Fact]
    public void SameSeed_GivesSameOrder()
    {
        var shuffle = new ShuffleHelper();
        var first = Enumerable.Range(0, 20).ToList();
        var second = Enumerable.Range(0, 20).ToList();

        shuffle.Shuffle(first, seed: 42);
        shuffle.Shuffle(second, seed: 42);

        Assert.Equal(first, second);
    }

    [Fact]
    public void Shuffle_KeepsAllElements()
    {
        var list = Enumerable.Range(0, 20).ToList();

        new ShuffleHelper().Shuffle(list);

        Assert.Equal(Enumerable.Range(0, 20), list.Order());
    }

    [Fact]
    public void NullList_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ShuffleHelper().Shuffle<int>(null!));
    }
    
    [Fact]
    public void TopScores_AreOrderedByMovesThenTime()
    {
        var repo = new InMemoryScoreRepository();
        repo.SaveScore(new Score { PlayerName = "slow", Moves = 10 });
        repo.SaveScore(new Score { PlayerName = "fast", Moves = 4 });

        var top = repo.GetTopScores(10).ToList();
        
        Assert.Equal("fast", top[0].PlayerName);
    }

    [Fact]
    public void TopScores_RespectLimit()
    {
        // Save 3 scores, ask for 2, and check the count
        var repo = new InMemoryScoreRepository();
        repo.SaveScore(new Score { PlayerName = "slow", Moves = 10 });
        repo.SaveScore(new Score() { PlayerName = "medium", Moves = 6 });
        repo.SaveScore(new Score { PlayerName = "fast", Moves = 4 });

        var top = repo.GetTopScores(2).ToList();

        Assert.Equal(2, top.Count);
    }

    [Fact]
    public void TopScores_BreakTiesByTime()
    {
        // two scores with equal moves and different TimeSeconds. Which should win? -- lower time wins
        var repo = new InMemoryScoreRepository();
        repo.SaveScore(new Score { PlayerName = "slow", Moves = 10, TimeSeconds = 100 });
        repo.SaveScore(new Score() { PlayerName = "super slow", Moves = 10, TimeSeconds = 200 });
        
        var top = repo.GetTopScores(2).ToList();
        
        Assert.Equal("slow", top[0].PlayerName);
    }
}

/// <summary>
/// Storage behaviour of <see cref="InMemoryScoreRepository"/>.
/// </summary>
public class InMemoryScoreRepositoryTests
{
    [Fact]
    public void SavedScores_AreReturned()
    {
        var repo = new InMemoryScoreRepository();

        repo.SaveScore(new Score { PlayerName = "a", Moves = 5 });
        repo.SaveScore(new Score { PlayerName = "b", Moves = 3 });

        Assert.Equal(2, repo.GetAllScores().Count());
    }
}

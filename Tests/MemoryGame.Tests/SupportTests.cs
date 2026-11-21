using MemoryGame.Backend.DataAccess.InMemory;
using MemoryGame.Backend.Models;
using MemoryGame.Backend.utilities;

namespace MemoryGame.Tests;

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
}

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

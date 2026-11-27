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
}
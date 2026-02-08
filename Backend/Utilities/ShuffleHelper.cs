using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MemoryGame.Backend.utilities;

public class ShuffleHelper
{
    public void Shuffle<T>(IList<T> list, int? seed = null)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        int n = list.Count;
        if (n <= 1) return;

        if (seed.HasValue)
        {
            var rng = new Random(seed.Value);
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        else
        {
            for (int i = n - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
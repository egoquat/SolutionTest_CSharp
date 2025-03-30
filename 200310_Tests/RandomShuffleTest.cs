using System;
using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtensions
{
    public static Dictionary<TKey, TValue> Shuffle<TKey, TValue>( this Dictionary<TKey, TValue> source)
    {
        Random r = new Random();
        return source.OrderBy(x => r.Next()).ToDictionary(item => item.Key, item => item.Value);
    }

    public static Dictionary<string, int> GetRandomShuffled()
    {
        Dictionary<string, int> source = new Dictionary<string, int>();
        for (int i = 0; i < 5; i++)
        {
            source.Add("Item " + i, i);
        }
        Dictionary<string, int> shuffled = source.Shuffle();
        return shuffled;
    }
}

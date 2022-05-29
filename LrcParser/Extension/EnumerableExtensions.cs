// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace LrcParser.Extension;

public static class EnumerableExtensions
{
    public static IEnumerable<IGrouping<TKey, TSource>> GroupByContinuous<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector) where TKey : IEquatable<TKey>
    {
        var tSources = source as TSource[] ?? source.ToArray();
        if (!tSources.Any())
            yield break;

        var list = new List<TSource>();
        TKey? previousKey = keySelector.Invoke(tSources.First());

        foreach (var item in tSources)
        {
            var key = keySelector.Invoke(item);

            if (equal(previousKey, key))
            {
                list.Add(item);
            }
            else
            {
                yield return new Grouping<TKey, TSource>(previousKey, list);

                previousKey = key;

                list.Clear();
                list.Add(item);
            }
        }

        if (list.Any())
        {
            yield return new Grouping<TKey, TSource>(previousKey, list);

            list.Clear();
        }

        static bool equal(TKey? key1, TKey? key2)
        {
            if (key1 == null)
                return key2 == null;

            return key2 != null && key1.Equals(key2);
        }
    }
}

public class Grouping<TKey, TElement> : List<TElement>, IGrouping<TKey, TElement>
{
    public Grouping(TKey key) => Key = key;
    public Grouping(TKey key, int capacity) : base(capacity) => Key = key;
    public Grouping(TKey key, IEnumerable<TElement> collection)
        : base(collection) => Key = key;
    public TKey Key { get; }
}

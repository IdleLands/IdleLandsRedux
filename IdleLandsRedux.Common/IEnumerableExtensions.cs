using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.Common
{
    [SuppressMessage("Gendarme.Rules.Naming", "UseCorrectPrefixRule")]
    // Taken from http://stackoverflow.com/questions/1779129/how-to-take-all-but-the-last-element-in-a-sequence-using-linq
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> SkipLastN<T>(this IEnumerable<T> source, int n)
        {
            if(n <= 0)
            {
                throw new ArgumentException(nameof(n));
            }
            
            using(var it = source.GetEnumerator())
            {
                bool hasRemainingItems = false;
                var cache = new Queue<T>(n + 1);
    
                do
                {
                    if (hasRemainingItems = it.MoveNext())
                    {
                        cache.Enqueue(it.Current);
                        if (cache.Count > n)
                            yield return cache.Dequeue();
                    }
                } while (hasRemainingItems);
            }
        }
    }
}
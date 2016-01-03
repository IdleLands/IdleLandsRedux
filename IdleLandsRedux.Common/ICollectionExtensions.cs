using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace IdleLandsRedux.Common
{
    [SuppressMessage("Gendarme.Rules.Naming", "UseCorrectPrefixRule")]
    // Taken from http://stackoverflow.com/questions/653596/how-to-conditionally-remove-items-from-a-net-collection
    public static class ICollectionExtensions
    {
        public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            if(collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            
            T element;

            for (int i = 0; i < collection.Count; i++)
            {
                element = collection.ElementAt(i);
                if (predicate(element))
                {
                    collection.Remove(element);
                    i--;
                }
            }
        }
    }
}
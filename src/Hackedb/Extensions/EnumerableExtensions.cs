using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackedb.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<TSource, TOther>(this IEnumerable<TSource> source,
            IEnumerable<TOther> others, Action<TSource, TOther> action)
        {
            TSource[] sourceArray = source.ToArray();
            TOther[]  othersArray = others.ToArray();
            for (var i = 0; i < Math.Min(sourceArray.Length, othersArray.Length); i++)
            {
                action(sourceArray[i], othersArray[i]);
            }
        }
    }
}

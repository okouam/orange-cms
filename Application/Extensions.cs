using System;
using System.Collections.Generic;
using System.Linq;

namespace OrangeCMS.Application
{
    public static class Extensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }

        public static IEnumerable<TSource> Sample<TSource>(this IEnumerable<TSource> source, int min, int max)
        {
            var items = source.ToArray();
            var num = new Random().Next(min, max);
            var result = new List<TSource>();

            num.Times(() => 
            {
                var category = items[new Random().Next(0, source.Count())];
                result.Add(category);
            });

            return result;
        }

        public static void Times(this int count, Action action)
        {
            for (var i = 0; i < count; i++)
            {
                action();
            }
        }
    }
}

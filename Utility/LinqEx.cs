using System;
using System.Collections.Generic;
using System.Linq;

namespace SvgViewer.Utility
{
    public static class LinqEx
    {
        public static TSource FindMin<TSource, TResult>(this IEnumerable<TSource> self , Func<TSource, TResult> selector)
        {
            var enumerable = self as TSource[] ?? self.ToArray();
            return enumerable.First(c => selector(c).Equals(enumerable.Min(selector)));
        }

        public static TSource FindMax<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
        {
            var enumerable = self as TSource[] ?? self.ToArray();
            return enumerable.First(c => selector(c).Equals(enumerable.Max(selector)));
        }
    }
}
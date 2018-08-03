using System;
using System.Collections.Generic;
using System.Linq;

namespace SvgViewer.Utility
{
    public static class LinqEx
    {
        public static TSource FindMin<TSource, TResult>(this IEnumerable<TSource> self , Func<TSource, TResult> selector)
        {
            return self.OrderBy(selector).FirstOrDefault();
        }

        public static TSource FindMax<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
        {
            return self.OrderBy(selector).LastOrDefault();
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace AutoFilter.Extensions
{
    public class CompiledExpressionsCache<TIn, TOut>
    {
        public static bool IsEnabled { get; set; } = false;

        private static readonly ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>> Cache
            = new ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>>();

        public static Func<TIn, TOut> AsFunc(Expression<Func<TIn, TOut>> expr)
        {
            if (!IsEnabled) return expr.Compile();

            return Cache.GetOrAdd(expr, k => k.Compile());
        }
    }
}

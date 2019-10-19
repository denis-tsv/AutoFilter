using System;
using System.Linq.Expressions;

namespace AutoFilter.Extensions
{
    public static class ExpressionExtensions
    {
        public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expr)
            => CompiledExpressionsCache<TIn, TOut>.AsFunc(expr);
    }
}

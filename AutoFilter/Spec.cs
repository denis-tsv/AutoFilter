using AutoFilter.Extensions;
using System;
using System.Linq.Expressions;

namespace AutoFilter;

public class Spec<T>
{
    public static bool operator false(Spec<T> _) => false;

    public static bool operator true(Spec<T> _) => false;

    public static Spec<T> operator &(Spec<T> spec1, Spec<T> spec2)
        => new(spec1.Expression.And(spec2.Expression));

    public static Spec<T> operator |(Spec<T> spec1, Spec<T> spec2)
        => new(spec1.Expression.Or(spec2.Expression));

    public static Spec<T> operator !(Spec<T> spec)
        => new(spec.Expression.Not());

    public static implicit operator Expression<Func<T, bool>>(Spec<T> spec)
        => spec.Expression;

    public static implicit operator Spec<T>(Expression<Func<T, bool>> expression)
        => new(expression);

    public Expression<Func<T, bool>> Expression { get; }

    public Spec(Expression<Func<T, bool>> expression)
    {
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }
}
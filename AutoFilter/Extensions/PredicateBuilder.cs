using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AutoFilter.Extensions
{
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that evaluates to true.
        /// </summary>
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        /// <summary>
        /// Creates a predicate that evaluates to false.
        /// </summary>
        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        /// <summary>
        /// Creates a predicate expression from the specified lambda expression.
        /// </summary>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose<Func<T, bool>>(second, Expression.AndAlso);
        }

        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose<Func<T, bool>>(second, Expression.OrElse);
        }

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        public static Expression<T> Compose<T>(this LambdaExpression first, LambdaExpression second,
            Func<Expression, Expression, Expression> merge)
        {
            var secondBody = ReplaceParameterExpressionVisitor.ReplaceParameters(first.Parameters.First(), second.Parameters.First(), second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<TFirstParam, TResult>> Combine<TFirstParam, TIntermediate, TResult>(
                        this Expression<Func<TFirstParam, TIntermediate>> first,
                    Expression<Func<TIntermediate, TResult>> second)
        {
            var param = Expression.Parameter(typeof(TFirstParam));

            var newFirst = first.Body.Replace(first.Parameters[0], param);
            var newSecond = second.Body.Replace(second.Parameters[0], newFirst);

            return Expression.Lambda<Func<TFirstParam, TResult>>(newSecond, param);
        }

        public static Expression<Func<TFirstParam, TResult>> Combine<TFirstParam, TIntermediate, TResult>(
                        this Expression<Func<TFirstParam, IEnumerable<TIntermediate>>> first,
                        Expression<Func<TIntermediate, TResult>> second)
        {
            var anyMethod = typeof(Enumerable).GetMethods().Single(x => x.Name == "Any" && x.GetParameters().Length == 2);
            var genericMethod = anyMethod.MakeGenericMethod(typeof(TIntermediate));

            var param = Expression.Parameter(typeof(TFirstParam));
            var firstBody = first.Body.Replace(first.Parameters[0], param);
            var res = Expression.Call(null, genericMethod, firstBody, second);

            return Expression.Lambda<Func<TFirstParam, TResult>>(res, param);
        }

        private static Expression Replace(this Expression expression, Expression searchEx, Expression replaceEx)
        {
            return new ReplaceExpressionVisitor(searchEx, replaceEx).Visit(expression);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _from, _to;

            public ReplaceExpressionVisitor(Expression from, Expression to)
            {
                _from = from;
                _to = to;
            }

            public override Expression Visit(Expression node)
            {
                return node == _from ? _to : base.Visit(node);
            }
        }

        private class ReplaceParameterExpressionVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _from, _to;

            private ReplaceParameterExpressionVisitor(ParameterExpression from, ParameterExpression to)
            {
                _from = from;
                _to = to;
            }
            
            public static Expression ReplaceParameters(ParameterExpression from, ParameterExpression to, Expression exp)
            {
                return new ReplaceParameterExpressionVisitor(from, to).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                return p == _from ? _to : base.Visit(p);
            }
        }
    }
}

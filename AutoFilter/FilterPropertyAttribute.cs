using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFilter
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterPropertyAttribute : Attribute
    {
        /// <summary>
        /// Default value for StringFilterCondition property. StringFilterCondition.StartsWith by default.
        /// </summary>
        public static StringFilterCondition DefaultStringFilterCondition = StringFilterCondition.StartsWith;
        
        /// <summary>
        /// Default value for IgnoreCase property. False by default.
        /// </summary>
        public static bool DefaultIgnoreCase = false;

        /// <summary>
        /// Default value for FilterCondition property. FilterCondition.Equal by default.
        /// </summary>
        public static FilterCondition DefaultFilterCondition = FilterCondition.Equal;

        protected static Expression NullConstant = Expression.Constant(null);

        public string? TargetPropertyName { get; set; }

        public StringFilterCondition StringFilter { get; set; } = DefaultStringFilterCondition;

        public bool IgnoreCase { get; set; } = DefaultIgnoreCase;

        public FilterCondition FilterCondition { get; set; } = DefaultFilterCondition;

        public virtual Expression GetExpression(ParameterExpression parameter, bool inMemory, PropertyInfo filterPropertyInfo, object filterPropertyValue, object filter)
        {
            if (filterPropertyValue is IRange range)
                return GetRangeExpression(parameter, inMemory, filterPropertyInfo, range, filter);

            if (filterPropertyValue is IEnumerable items && filterPropertyValue.GetType() != typeof(string))
                return GetEnumerableExpression(parameter, inMemory, filterPropertyInfo, items, filter);

            return GetScalarExpression(parameter, inMemory, filterPropertyInfo, filterPropertyValue, filter);
        }

        protected Expression GetRangeExpression(ParameterExpression parameter, bool inMemory, PropertyInfo filterPropertyInfo, IRange range, object filter)
        {
            var @from = range.From;
            var @to = range.To;
            if (@from == null && @to == null) throw new ArgumentException("Range From and To are empty");

            Expression? fromExpr = null, toExpr = null;
            if (@from != null)
            {
                var property = GetPropertyExpression(parameter, filterPropertyInfo);
                Expression value = Expression.Constant(@from);
                if (value.Type != property.Type)
                    value = Expression.Convert(value, property.Type); //to convert from enum to object or from int? to int

                fromExpr = GetBody(property, value, inMemory, FilterCondition.GreaterOrEqual);

                fromExpr = AddNullChecks(fromExpr, inMemory, parameter, property);

                if (@to == null) return fromExpr;
            }

            if (@to != null)
            {
                var property = GetPropertyExpression(parameter, filterPropertyInfo);
                Expression value = Expression.Constant(@to);
                if (value.Type != property.Type)
                    value = Expression.Convert(value, property.Type); //to convert from enum to object or from int? to int
                toExpr = GetBody(property, value, inMemory, FilterCondition.LessOrEqual);
                toExpr = AddNullChecks(toExpr, inMemory, parameter, property);

                if (@from == null) return toExpr;
            }

            return Expression.AndAlso(fromExpr!, toExpr!);
        }

        protected Expression GetEnumerableExpression(ParameterExpression parameter, bool inMemory, PropertyInfo filterPropertyInfo, IEnumerable items, object filter)
        {
            Expression property = GetPropertyExpression(parameter, filterPropertyInfo);
            Expression itemsExpr = Expression.Constant(items);

            var (containsMethodInfo, elementType) = EnumerablePropertyCache.GetInfo(filterPropertyInfo);
            
            var convertedProperty = elementType != property.Type ? 
                Expression.Convert(property, elementType) : //to convert from enum to object or from int? to int
                property;

            Expression result = Expression.Call(null, containsMethodInfo, itemsExpr, convertedProperty);

            result = AddNullChecks(result, inMemory, parameter, property, true);

            return result;
        }

        protected Expression GetScalarExpression(ParameterExpression parameter, bool inMemory, PropertyInfo filterPropertyInfo, object filterPropertyValue, object filter)
        {
            var property = GetPropertyExpression(parameter, filterPropertyInfo);
            var propertyValue = GetPropertyValue(filterPropertyValue, filter);

            Expression value = Expression.Constant(propertyValue);
            if (value.Type != property.Type)
                value = Expression.Convert(value, property.Type); //to convert from enum to object or from int? to int

            var body = GetBody(property, value, inMemory);

            body = AddNullChecks(body, inMemory, parameter, property);

            return body;
        }

        protected Expression AddNullChecks(Expression body, bool inMemory, ParameterExpression parameter, Expression property, bool isEnumerable = false)
        {
            if (!inMemory) return body;

            var nullChecks = new List<Expression>();

            var nestedNullCheck = GetNestedNullCheckExpression(parameter);
            if (nestedNullCheck != null) nullChecks.Add(nestedNullCheck);

            //for Nullable ValueType we don't generate a null check because it don't needed
            if ((!property.Type.IsValueType && !isEnumerable) || 
                (property.Type.IsValueType && isEnumerable && property.Type.Name.StartsWith("Nullable")))
            {
                var propertyNullCheck = GetNullCheckExpression(property);
                nullChecks.Add(propertyNullCheck);
            }
            if (nullChecks.Any())
            {
                nullChecks.Add(body);
                body = nullChecks.Aggregate(Expression.AndAlso);
            }

            return body;
        }

        protected virtual Expression GetNullCheckExpression(Expression propertyExpression)
        {
            return Expression.NotEqual(propertyExpression, NullConstant);
        }

        protected virtual Expression? GetNestedNullCheckExpression(ParameterExpression parameter)
        {
            return null;
        }

        protected virtual object GetPropertyValue(object filterPropertyValue, object filter)
        {
            return filterPropertyValue;
        }
                
        protected virtual string GetPropertyName(PropertyInfo filterPropertyInfo)
        {
            return TargetPropertyName ?? filterPropertyInfo.Name;
        }

        protected virtual Expression GetBody(MemberExpression property, Expression value, bool inMemory, FilterCondition? condition = null)
        {
            var func = GetBodyBuilderFunc(property.Type, condition);

            return func(property, value);
        }

        protected virtual MemberExpression GetPropertyExpression(ParameterExpression parameter, PropertyInfo filterPropertyInfo)
        {
            return Expression.Property(parameter, GetPropertyName(filterPropertyInfo));
        }

        protected virtual Func<MemberExpression, Expression, Expression> GetBodyBuilderFunc(Type propertyType, FilterCondition? condition)
        {
            if (propertyType == typeof(string))
                return GetStringBuilderFunc();
            
            var cond = condition ?? FilterCondition;

            return cond switch
            {
                FilterCondition.Equal => Expression.Equal,
                FilterCondition.Greater => Expression.GreaterThan,
                FilterCondition.GreaterOrEqual => Expression.GreaterThanOrEqual,
                FilterCondition.Less => Expression.LessThan,
                FilterCondition.LessOrEqual => Expression.LessThanOrEqual,
                _ => Expression.Equal
            };
        }
        
        private static readonly Func<MemberExpression, Expression, Expression> StringStartWithFunc;
        private static readonly Func<MemberExpression, Expression, Expression> StringContainsFunc;
        private static readonly Func<MemberExpression, Expression, Expression> StringStartWithIgnoreCaseFunc;
        private static readonly Func<MemberExpression, Expression, Expression> StringContainsIgnoreCaseFunc;

        static FilterPropertyAttribute()
        {

            var startsWith = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) });
            var contains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
            var toLower = typeof(string).GetMethod(nameof(string.ToLower), Array.Empty<Type>());

            StringStartWithFunc = (p, v) => Expression.Call(p, startsWith!, v);
            StringContainsFunc = (p, v) => Expression.Call(p, contains!, v);

            StringStartWithIgnoreCaseFunc = (p, v) =>
            {
                var pl = Expression.Call(p, toLower!);
                var vl = Expression.Call(v, toLower!);
                return Expression.Call(pl, startsWith!, vl);
            };

            StringContainsIgnoreCaseFunc = (p, v) =>
            {
                var pl = Expression.Call(p, toLower!);
                var vl = Expression.Call(v, toLower!);
                return Expression.Call(pl, contains!, vl);
            };
        }

        protected virtual Func<MemberExpression, Expression, Expression> GetStringBuilderFunc()
        {
            return (StringFilter, IgnoreCase) switch
            {
                (StringFilterCondition.StartsWith, true)  => StringStartWithIgnoreCaseFunc,
                (StringFilterCondition.StartsWith, false) => StringStartWithFunc,
                (StringFilterCondition.Contains, true)    => StringContainsIgnoreCaseFunc,
                (StringFilterCondition.Contains, false)   => StringContainsFunc,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

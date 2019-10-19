using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFilter
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FilterPropertyAttribute : Attribute
    {
        protected static Expression NullConstant = Expression.Constant(null);

        public string TargetPropertyName { get; set; }

        public StringFilterCondition StringFilter { get; set; } 

        public bool IgnoreCase { get; set; }

        public FilterCondition FilterCondition { get; set; }

        public virtual Expression<Func<TItem, bool>> GetExpression<TItem>(bool inMemory, PropertyInfo filterPropertyInfo, object filter)
        {
            var parameter = Expression.Parameter(typeof(TItem));
            var property = GetPropertyExpression(parameter, filterPropertyInfo);
            var propertyValue = GetPropertyValue(filterPropertyInfo, filter);

            Expression value = Expression.Constant(propertyValue);

            value = Expression.Convert(value, property.Type); //to convert from enum to object or from int? to int

            var body = GetBody(property, value, inMemory);

            if (inMemory)
            {
                var nullChecks = new List<Expression>();

                var nestedNullCheck = GetNestedNullCheckExpression(parameter);
                if (nestedNullCheck != null) nullChecks.Add(nestedNullCheck);

                //for Nullable ValueType we don't generate a null check because it don't needed
                if (!property.Type.IsValueType)
                {
                    var propertyNullCheck = GetNullCheckExpression(property);
                    nullChecks.Add(propertyNullCheck);
                }
                if (nullChecks.Any())
                {
                    nullChecks.Add(body);
                    body = nullChecks.Aggregate((x, y) => Expression.AndAlso(x, y));
                }
            }

            var res = Expression.Lambda<Func<TItem, bool>>(body, parameter);

            return res;                      
        }

        protected virtual Expression GetNullCheckExpression(Expression propertyExpression)
        {
            return Expression.NotEqual(propertyExpression, NullConstant);
        }

        protected virtual Expression GetNestedNullCheckExpression(ParameterExpression parameter)
        {
            return null;
        }

        protected virtual object GetPropertyValue(PropertyInfo filterPropertyInfo, object filter)
        {
            return filterPropertyInfo.GetValue(filter);
        }
                
        protected virtual string GetPropertyName(PropertyInfo filterPropertyInfo)
        {
            return TargetPropertyName ?? filterPropertyInfo.Name;
        }

        protected virtual Expression GetBody(MemberExpression property, Expression value, bool inMemory)
        {
            var func = GetBodyBuilderFunc(property.Type);

            return func(property, value);
        }

        protected virtual MemberExpression GetPropertyExpression(ParameterExpression parameter, PropertyInfo filterPropertyInfo)
        {
            return Expression.Property(parameter, GetPropertyName(filterPropertyInfo));
        }

        protected virtual Func<MemberExpression, Expression, Expression> GetBodyBuilderFunc(Type propertyType)
        {
            if (propertyType == typeof(string))
                return GetStringBuilderFunc();

            switch (FilterCondition)
            {
                case FilterCondition.Equal: return Expression.Equal;
                case FilterCondition.Greater: return Expression.GreaterThan;
                case FilterCondition.GreaterOrEqual: return Expression.GreaterThanOrEqual;
                case FilterCondition.Less: return Expression.LessThan;
                case FilterCondition.LessOrEqual: return Expression.LessThanOrEqual;
            }

            return Expression.Equal;        
        }
        
        private static readonly string StringStartWithIgnoreCase = "StringStartWithIgnoreCase";
        private static readonly string StringStartWith = "StringStartWith";
        private static readonly string StringContainsIgnoreCase = "StringContainsIgnoreCase";
        private static readonly string StringContains = "StringContains";

        private static readonly IReadOnlyDictionary<string, Func<MemberExpression, Expression, Expression>> _stringFilters;            

        static FilterPropertyAttribute()
        {
            var filters = new Dictionary<string, Func<MemberExpression, Expression, Expression>>();

            var startsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var contains = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            filters.Add(StringStartWith, (p, v) => Expression.Call(p, startsWith, v));
            filters.Add(StringContains, (p, v) => Expression.Call(p, contains, v));

            Func<MemberExpression, Expression, Expression> startIgnoreCase = (p, v) =>
            {
                var mi = typeof(string).GetMethod("ToLower", new Type[] { });
                var pl = Expression.Call(p, mi);
                var vl = Expression.Call(v, mi);
                return Expression.Call(pl, startsWith, vl);
            };
            filters.Add(StringStartWithIgnoreCase, startIgnoreCase);

            Func<MemberExpression, Expression, Expression> containsIgnoreCase = (p, v) =>
            {
                var mi = typeof(string).GetMethod("ToLower", new Type[] { });
                var pl = Expression.Call(p, mi);
                var vl = Expression.Call(v, mi);
                return Expression.Call(pl, contains, vl);
            };
            filters.Add(StringContainsIgnoreCase, containsIgnoreCase);
            _stringFilters = filters;
        }

        protected virtual Func<MemberExpression, Expression, Expression> GetStringBuilderFunc()
        {
            //TODO use C# 8 tuple patterns
            switch (StringFilter)
            {
                case StringFilterCondition.StartsWith:
                    if (IgnoreCase)
                        return _stringFilters[StringStartWithIgnoreCase];
                    else
                        return _stringFilters[StringStartWith];                    
                case StringFilterCondition.Contains:
                    if (IgnoreCase)
                        return _stringFilters[StringContainsIgnoreCase];
                    else
                        return _stringFilters[StringContains];                    
            }

            throw new InvalidOperationException();
        }
    }
}

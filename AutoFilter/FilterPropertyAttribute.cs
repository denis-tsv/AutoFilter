using System;
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

        public string TargetPropertyName { get; set; }

        public StringFilterCondition StringFilter { get; set; } = DefaultStringFilterCondition;

        public bool IgnoreCase { get; set; } = DefaultIgnoreCase;

        public FilterCondition FilterCondition { get; set; } = DefaultFilterCondition;

        public virtual Expression GetExpression(ParameterExpression parameter, bool inMemory, PropertyInfo filterPropertyInfo, object filterPropertyValue, object filter)
        {
            var property = GetPropertyExpression(parameter, filterPropertyInfo);
            var propertyValue = GetPropertyValue(filterPropertyValue, filter);

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
                    body = nullChecks.Aggregate(Expression.AndAlso);
                }
            }

            return body;                      
        }

        protected virtual Expression GetNullCheckExpression(Expression propertyExpression)
        {
            return Expression.NotEqual(propertyExpression, NullConstant);
        }

        protected virtual Expression GetNestedNullCheckExpression(ParameterExpression parameter)
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

            return FilterCondition switch
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
            var toLower = typeof(string).GetMethod(nameof(string.ToLower), new Type[] { });

            StringStartWithFunc = (p, v) => Expression.Call(p, startsWith, v);
            StringContainsFunc = (p, v) => Expression.Call(p, contains, v);

            StringStartWithIgnoreCaseFunc = (p, v) =>
            {
                var pl = Expression.Call(p, toLower);
                var vl = Expression.Call(v, toLower);
                return Expression.Call(pl, startsWith, vl);
            };

            StringContainsIgnoreCaseFunc = (p, v) =>
            {
                var pl = Expression.Call(p, toLower);
                var vl = Expression.Call(v, toLower);
                return Expression.Call(pl, contains, vl);
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

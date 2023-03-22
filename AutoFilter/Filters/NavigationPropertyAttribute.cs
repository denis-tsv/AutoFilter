﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFilter.Filters;

public class NavigationPropertyAttribute : FilterPropertyAttribute
{
    public NavigationPropertyAttribute(string objectName)
    {
        Path = objectName ?? throw new ArgumentNullException(nameof(objectName));
    }

    public string Path { get; }

    protected override Expression GetNestedNullCheckExpression(ParameterExpression parameter)
    {  
        var nullChecks = new List<Expression>();
        var propNames = Path.Split('.');
        var property = Expression.Property(parameter, propNames[0]);

        var nullCheck = Expression.NotEqual(property, NullConstant);
        nullChecks.Add(nullCheck);

        for (int i = 1; i < propNames.Length; i++)
        {
            property = Expression.Property(property, propNames[i]);
            nullCheck = Expression.NotEqual(property, NullConstant);
            nullChecks.Add(nullCheck);                    
        }

        var aggregatedNullChecks = nullChecks.Aggregate((cur, next) => Expression.AndAlso(cur, next));
        return aggregatedNullChecks;
    }

    protected override MemberExpression GetPropertyExpression(ParameterExpression parameter, PropertyInfo filterPropertyInfo)
    {
        var propNames = Path.Split('.');
            
        var property = Expression.Property(parameter, propNames[0]);
            
        for (int i = 1; i < propNames.Length; i++)
        {
            property = Expression.Property(property, propNames[i]);                
        }

        return Expression.Property(property, GetPropertyName(filterPropertyInfo));
    }
}
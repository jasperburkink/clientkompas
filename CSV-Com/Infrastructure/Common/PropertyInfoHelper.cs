﻿using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Common
{
    public static class PropertyInfoHelper
    {
        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            if (propertyLambda.Body is not MemberExpression member)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            if (member.Member is not PropertyInfo propInfo)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            var type = typeof(TSource);
            if (propInfo.ReflectedType != null && type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a property that is not from type {type}.");
            }

            return propInfo;
        }
    }
}

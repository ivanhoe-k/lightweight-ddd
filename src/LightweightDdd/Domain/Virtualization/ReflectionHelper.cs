// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LightweightDdd.Domain.Virtualization
{
    internal static class ReflectionHelper
    {
        public static Type GetExpressionConstructorType(Type entityType, Type propertyType)
        {
            entityType.ThrowIfNull();
            propertyType.ThrowIfNull();

            return typeof(Expression<>).MakeGenericType(
                typeof(Func<,>).MakeGenericType(entityType, propertyType));
        }

        public static Type[] GetVirtualPropertyResolvedConstructorTypes(Type propertyType)
        {
            propertyType.ThrowIfNull();

            return new[] { typeof(string),  typeof(string),propertyType };
        }

        public static ConstructorInfo? GetVirtualPropertyConstructorOrThrow(
            Type entityType,
            Type propertyType,
            Type virtualPropertyType,
            bool isExpressionCtor,
            BindingFlags bindingFlags)
        {
            virtualPropertyType.ThrowIfNull();
            entityType.ThrowIfNull();
            propertyType.ThrowIfNull();

            var args = isExpressionCtor
                ? new[] { GetExpressionConstructorType(entityType, propertyType) }
                : GetVirtualPropertyResolvedConstructorTypes(propertyType);

            return GetConstructor(virtualPropertyType, args, bindingFlags);
        }

        public static ConstructorInfo? GetConstructor(Type type, Type[] parameterTypes, BindingFlags bindingFlags)
        {
            type.ThrowIfNull();
            parameterTypes.ThrowIfNull();

            var ctor = type.GetConstructor(
                bindingFlags,
                binder: null,
                types: parameterTypes,
                modifiers: null);

            return ctor;
        }
    }
}

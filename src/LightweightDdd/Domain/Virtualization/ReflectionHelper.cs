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
        public static Type[] GetUnresolvedConstructorTypes()
        {
            return new[] { typeof(string), typeof(string) };
        }

        public static Type[] GetResolvedConstructorTypes(Type propertyType)
        {
            propertyType.ThrowIfNull();

            return new[] { typeof(string), typeof(string), propertyType };
        }

        public static Type[] GetVirtualPropertyConstructorTypes(Type propertyType, bool isUnresolvedCtor)
        {
            propertyType.ThrowIfNull();

            return isUnresolvedCtor
                ? GetUnresolvedConstructorTypes()
                : GetResolvedConstructorTypes(propertyType);
        }

        public static ConstructorInfo? GetVirtualPropertyConstructor(
            Type propertyType,
            Type virtualPropertyType,
            bool isUnresolvedCtor,
            BindingFlags bindingFlags)
        {
            propertyType.ThrowIfNull();
            virtualPropertyType.ThrowIfNull();

            return GetConstructor(
                type: virtualPropertyType,
                parameterTypes: GetVirtualPropertyConstructorTypes(propertyType, isUnresolvedCtor),
                bindingFlags: bindingFlags);
        }

        private static ConstructorInfo? GetConstructor(Type type, Type[] parameterTypes, BindingFlags bindingFlags)
        {
            type.ThrowIfNull();
            parameterTypes.ThrowIfNull();

            return type.GetConstructor(
                bindingAttr: bindingFlags,
                binder: null,
                types: parameterTypes,
                modifiers: null);
        }
    }
}

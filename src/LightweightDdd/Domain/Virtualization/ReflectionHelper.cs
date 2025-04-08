// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LightweightDdd.Domain.Virtualization
{
    /// <summary>
    /// Provides internal helper methods for resolving non-public constructors
    /// required by the Virtual Property system using reflection.
    /// </summary>
    /// <remarks>
    /// This utility is used exclusively by <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}"/> and its subclasses
    /// to support dynamic instantiation of virtual property types via reflection, while enforcing constructor signature expectations.
    /// </remarks>
    internal static class ReflectionHelper
    {
        /// <summary>
        /// Gets the expected parameter types for the unresolved constructor: (string entityName, string propertyName).
        /// </summary>
        /// <returns>An array of constructor parameter types used for unresolved virtual property instantiation.</returns>
        public static Type[] GetUnresolvedConstructorTypes()
        {
            return new[] { typeof(string), typeof(string) };
        }

        /// <summary>
        /// Gets the expected parameter types for the resolved constructor:
        /// (string entityName, string propertyName, TProperty value).
        /// </summary>
        /// <param name="propertyType">The type of the property being virtualized.</param>
        /// <returns>An array of constructor parameter types used for resolved virtual property instantiation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyType"/> is null.</exception>
        public static Type[] GetResolvedConstructorTypes(Type propertyType)
        {
            propertyType.ThrowIfNull();

            return new[] { typeof(string), typeof(string), typeof(bool), propertyType };
        }

        /// <summary>
        /// Returns the expected constructor parameter types for either the unresolved or resolved virtual property constructor.
        /// </summary>
        /// <param name="propertyType">The type of the virtualized value.</param>
        /// <param name="isUnresolvedCtor">
        /// If <c>true</c>, returns the unresolved constructor signature: (string entityName, string propertyName);  
        /// otherwise, returns the resolved signature: (string entityName, string propertyName, TProperty value).
        /// </param>
        /// <returns>The expected constructor parameter types.</returns>
        public static Type[] GetVirtualPropertyConstructorTypes(Type propertyType, bool isUnresolvedCtor)
        {
            propertyType.ThrowIfNull();

            return isUnresolvedCtor
                ? GetUnresolvedConstructorTypes()
                : GetResolvedConstructorTypes(propertyType);
        }

        /// <summary>
        /// Resolves the appropriate non-public constructor for the virtual property type using the expected signature.
        /// </summary>
        /// <param name="propertyType">The type of the virtualized value.</param>
        /// <param name="virtualPropertyType">The CLR type of the virtual property (e.g. a subclass of <c>VirtualPropertyBase&lt;...&gt;</c>).</param>
        /// <param name="isUnresolvedCtor">
        /// If <c>true</c>, looks for the unresolved constructor (entityName, propertyName);
        /// otherwise, looks for the resolved constructor (entityName, propertyName, value).
        /// </param>
        /// <param name="bindingFlags">The binding flags to use for constructor lookup (should be <c>NonPublic | Instance</c>).</param>
        /// <returns>A <see cref="ConstructorInfo"/> instance if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="propertyType"/> or <paramref name="virtualPropertyType"/> is null.
        /// </exception>
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

        /// <summary>
        /// Returns a non-public constructor of the given type that matches the specified parameter types.
        /// </summary>
        /// <param name="type">The type to inspect for constructors.</param>
        /// <param name="parameterTypes">The parameter types to match.</param>
        /// <param name="bindingFlags">Binding flags to use during constructor lookup.</param>
        /// <returns>The matching <see cref="ConstructorInfo"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> or <paramref name="parameterTypes"/> is null.</exception>
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

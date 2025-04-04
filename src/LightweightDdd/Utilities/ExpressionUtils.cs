// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LightweightDdd.Utilities
{
    internal static class ExpressionUtils
    {
        /// <summary>
        /// Extracts the full property path from a lambda expression, such as <c>x => x.A.B.C</c>,
        /// and returns it as a dot-delimited string (e.g., "A.B.C").
        /// </summary>
        /// <typeparam name="TEntity">The type of the root object in the expression.</typeparam>
        /// <typeparam name="TProperty">The type of the final property being accessed.</typeparam>
        /// <param name="expression">
        /// A lambda expression representing a property access chain starting from the root entity.
        /// For example: <c>entity => entity.SubEntity.SomeProperty</c>.
        /// </param>
        /// <returns>
        /// A string containing the full property path in dot notation (e.g., <c>"SubEntity.SomeProperty"</c>).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the provided expression is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the expression does not consist of a chain of property or field accesses
        /// starting from a parameter expression.
        /// </exception>
        public static string GetPropertyPath<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            expression.ThrowIfNull();

            var parts = new Stack<string>();
            var current = expression.Body;

            while (current is MemberExpression memberExpr)
            {
                parts.Push(memberExpr.Member.Name);
                current = memberExpr.Expression;
            }

            if (current is not ParameterExpression)
            {
                throw new InvalidOperationException("Expression must be a chain of member accesses starting from a parameter (e.g., x => x.Prop.SubProp)");
            }

            return string.Join(".", parts);
        }
    }
}

// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LightweightDdd.Core.Extensions
{
    public static class GuardExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the provided <paramref name="argument"/> is null.
        /// </summary>
        /// <param name="argument">The object to check for null.</param>
        /// <param name="paramName">The name of the parameter (automatically generated if not provided).</param>
        public static void ThrowIfNull(this object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument != null)
            {
                return;
            }

            var exception = new ArgumentNullException(paramName);
            throw exception;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the provided <paramref name="argument"/> is null or empty.
        /// </summary>
        /// <param name="argument">The string to check for null or empty.</param>
        /// <param name="paramName">The name of the parameter (automatically generated if not provided).</param>
        public static void ThrowIfNullOrWhiteSpace(this string argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (!string.IsNullOrWhiteSpace(argument))
            {
                return;
            }

            var exception = new ArgumentNullException(paramName);
            throw exception;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the provided <paramref name="argument"/> is <see cref="Guid.Empty"/>.
        /// </summary>
        /// <param name="argument">The Guid to check for empty.</param>
        /// <param name="paramName">The name of the parameter (automatically generated if not provided).</param>
        public static void ThrowIfEmpty(this Guid argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument != Guid.Empty)
            {
                return;
            }

            var exception = new ArgumentNullException(paramName);
            throw exception;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the provided value is null or its default value.
        /// Supports both reference and value types.
        /// </summary>
        /// <typeparam name="T">The type of the value to check.</typeparam>
        /// <param name="argument">The value to check.</param>
        /// <param name="paramName">The name of the parameter (automatically generated).</param>
        public static void ThrowIfNullOrDefault<T>(this T argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(paramName, $"The value '{paramName}' cannot be null.");
            }

            if (EqualityComparer<T>.Default.Equals(argument, default))
            {
                throw new ArgumentException($"The value '{paramName}' must not be the default value.", paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if a condition is true.
        /// </summary>
        /// <param name="argument">The object to which the condition applies.</param>
        /// <param name="condition">The condition that, if true, triggers the exception.</param>
        /// <param name="errorMessage">The error message to include in the exception.</param>
        /// <param name="paramName">The name of the parameter being checked (automatically inferred).</param>
        public static void ThrowIf(
            this object argument,
            bool condition,
            string errorMessage,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (!condition)
            {
                return;
            }

            var exception = new ArgumentException(errorMessage, paramName);
            throw exception;
        }
    }
}

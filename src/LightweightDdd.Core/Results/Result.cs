// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using LightweightDdd.Core.Extensions;

namespace LightweightDdd.Core.Results
{
    /// <summary>
    /// Represents the outcome of an operation that may either succeed or fail, without returning a value.
    /// </summary>
    /// <typeparam name="TError">The type of the error in case of failure. Must implement <see cref="IError"/>.</typeparam>
    public class Result<TError>
        where TError : IError
    {
        /// <summary>
        /// Initializes a successful result.
        /// </summary>
        protected Result()
        {
        }

        /// <summary>
        /// Initializes a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error associated with the failed result.</param>
        protected Result(TError error)
        {
            Error = error;
        }

        /// <summary>
        /// Gets the error associated with the result, if any.
        /// </summary>
        public TError? Error { get; }

        /// <summary>
        /// Gets a value indicating whether the result represents a successful outcome.
        /// </summary>
        public bool Succeeded => !Failed;

        /// <summary>
        /// Gets a value indicating whether the result represents a failure.
        /// </summary>
        public bool Failed => Error != null;

        /// <summary>
        /// Creates a successful result with no return value.
        /// </summary>
        /// <returns>A successful <see cref="Result{TError}"/> instance.</returns>
        public static Result<TError> Ok()
        {
            return new Result<TError>();
        }

        /// <summary>
        /// Creates a successful result with a return value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="result">The result value.</param>
        /// <returns>A successful <see cref="Result{TError, TResult}"/> containing the value.</returns>
        public static Result<TError, TResult> Ok<TResult>(TResult result)
        {
            return new Result<TError, TResult>(result);
        }

        /// <summary>
        /// Creates a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error to associate with the failed result.</param>
        /// <returns>A failed <see cref="Result{TError}"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="error"/> is null.</exception>
        public static Result<TError> Fail(TError error)
        {
            error.ThrowIfNull();
            return new Result<TError>(error);
        }

        /// <summary>
        /// Creates a failed result with the specified error and a typed return value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result value (which will be absent due to failure).</typeparam>
        /// <param name="error">The error to associate with the failed result.</param>
        /// <returns>A failed <see cref="Result{TError, TResult}"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="error"/> is null.</exception>
        public static Result<TError, TResult> Fail<TResult>(TError error)
        {
            error.ThrowIfNull();
            return new Result<TError, TResult>(error);
        }
    }
}

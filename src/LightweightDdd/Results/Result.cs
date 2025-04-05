// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using LightweightDdd.Extensions;

namespace LightweightDdd.Results
{
    /// <summary>
    /// Represents the outcome of an operation that can either fail with an error,
    /// or succeed without producing a result value.
    /// </summary>
    /// <typeparam name="TError">The non-nullable type used to represent an error in the failure case.</typeparam>
    /// <remarks>
    /// This type is conceptually inspired by the notion of <c>Unit</c> in functional programming,
    /// where success carries no data — it simply means "the operation completed successfully".
    /// It models a command-style result that signals success or failure explicitly,
    /// without relying on exceptions for flow control.
    /// <para>
    /// Use <see cref="Success"/> to construct a successful result, or <see cref="Fail(TError)"/> to create a failed one.
    /// Accessing <see cref="Error"/> when the result has succeeded — or accessing any property before proper initialization —
    /// will throw a specific exception to enforce correct usage.
    /// </para>
    /// <para>
    /// This struct is immutable, allocation-free, and safe by design. Constructing it using <c>default</c>
    /// is not allowed and will result in an <see cref="UninitializedResultException"/> when accessed.
    /// </para>
    /// <para>
    /// The <typeparamref name="TError"/> type is constrained to be non-nullable (<c>notnull</c>)
    /// to enforce valid failure states and avoid runtime null reference errors.
    /// </para>
    /// </remarks>
    public readonly record struct Result<TError> : IResult<TError>
        where TError : notnull
    {
        private readonly TError _error;
        private readonly bool _initialized;
        private readonly bool _failed;

        private Result(bool initialized)
        {
            _initialized = initialized;
        }

        private Result(TError error) : this(true)
        {
            error.ThrowIfNull();

            _error = error;
            _failed = true;
        }

        /// <summary>
        /// Gets the error associated with a failed result.
        /// </summary>
        /// <exception cref="ResultSucceededException">
        /// Thrown if the result has succeeded and therefore does not contain an error.
        /// </exception>
        /// <exception cref="UninitializedResultException">
        /// Thrown if the result was not properly initialized.
        /// </exception>
        public TError Error => ResultGuard.ExtractError(Succeeded, _initialized, _error);

        /// <summary>
        /// Gets a value indicating whether the result has failed.
        /// </summary>
        /// <exception cref="UninitializedResultException">
        /// Thrown if the result was not constructed through a factory method.
        /// </exception>
        public bool Failed => ResultGuard.EnsureInitializedAndReturnFailureState(_failed, _initialized);

        /// <summary>
        /// Gets a value indicating whether the result has succeeded.
        /// </summary>
        /// <exception cref="UninitializedResultException">
        /// Thrown if the result was not constructed through a factory method.
        /// </exception>
        public bool Succeeded => !ResultGuard.EnsureInitializedAndReturnFailureState(_failed, _initialized);

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>A result that indicates success and contains no error.</returns>
        public static Result<TError> Success()
        {
            return new Result<TError>(true);
        }

        /// <summary>
        /// Creates a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error that caused the failure.</param>
        /// <returns>
        /// A <see cref="Result{TError}"/> that represents a failed outcome containing the provided error.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="error"/> is <c>null</c>.
        /// </exception>
        public static Result<TError> Fail(TError error)
        {
            return new Result<TError>(error);
        }

        /// <summary>
        /// Creates a successful result with the specified value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value associated with a successful result.</typeparam>
        /// <param name="value">The value to include in the success case.</param>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> that represents a successful outcome with the specified value.
        /// </returns>
        public static Result<TError, TValue> Success<TValue>(TValue value)
        {
            return new Result<TError, TValue>(value);
        }

        /// <summary>
        /// Creates a failed result with the specified error.
        /// </summary>
        /// <typeparam name="TValue">The type that would be returned in the success case.</typeparam>
        /// <param name="error">The error that caused the failure.</param>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> that represents a failed outcome containing the provided error.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="error"/> is <c>null</c>.
        /// </exception>
        public static Result<TError, TValue> Fail<TValue>(TError error)
        {
            return new Result<TError, TValue>(error);
        }
    }

    /// <summary>
    /// Represents the outcome of an operation that can either fail with an error,
    /// or succeed with a result value.
    /// </summary>
    /// <typeparam name="TError">The non-nullable type used to represent an error in the failure case.</typeparam>
    /// <typeparam name="TValue">The type of the value produced when the operation succeeds.</typeparam>
    /// <remarks>
    /// This type is conceptually inspired by the "Result" or "Either" monads found in functional programming,
    /// where a computation yields either a value (on success) or an error (on failure).
    /// It models a result-producing operation that signals success or failure explicitly,
    /// without relying on exceptions for control flow.
    /// <para>
    /// Use <see cref="Success(TValue)"/> to construct a successful result, or <see cref="Fail(TError)"/> to create a failed one.
    /// Accessing <see cref="Value"/> when the result has failed — or <see cref="Error"/> when it has succeeded —
    /// will throw a specific exception to enforce correct usage.
    /// </para>
    /// <para>
    /// This struct is immutable, allocation-free, and safe by design. Constructing it using <c>default</c>
    /// is not allowed and will result in an <see cref="UninitializedResultException"/> when accessed.
    /// </para>
    /// <para>
    /// The <typeparamref name="TError"/> type is constrained to be non-nullable (<c>notnull</c>)
    /// to enforce valid failure states and avoid runtime null reference errors.
    /// </para>
    /// </remarks>
    public readonly record struct Result<TError, TValue> : IResult<TError, TValue>
         where TError : notnull
    {
        private readonly TError _error;
        private readonly TValue _value;
        private readonly bool _initialized;
        private readonly bool _failed;

        private Result(bool initialized)
        {
            _initialized = initialized;
        }

        internal Result(TValue value) : this(true)
        {
            _value = value;
            _failed = false;
        }

        internal Result(TError error) : this(true)
        {
            error.ThrowIfNull();

            _failed = true;
            _error = error;
        }

        /// <summary>
        /// Gets the error associated with a failed result.
        /// </summary>
        /// <exception cref="ResultSucceededException">
        /// Thrown if the result has succeeded and therefore does not contain an error.
        /// </exception>
        /// <exception cref="UninitializedResultException">
        /// Thrown if the result was not properly initialized using a factory method.
        /// </exception>
        public TError Error => ResultGuard.ExtractError(Succeeded, _initialized, _error);

        /// <summary>
        /// Gets the value produced by a successful result.
        /// </summary>
        /// <exception cref="ResultFailedException">
        /// Thrown if the result has failed and therefore does not contain a value.
        /// </exception>
        /// <exception cref="UninitializedResultException">
        /// Thrown if the result was not properly initialized using a factory method.
        /// </exception>
        public TValue Value => ResultGuard.ExtractValue(Succeeded, _initialized, _value);

        /// <summary>
        /// Gets a value indicating whether the result has failed.
        /// </summary>
        /// <remarks>
        /// If the result was not properly initialized, accessing this property will throw
        /// an <see cref="UninitializedResultException"/>.
        /// </remarks>
        /// <exception cref="UninitializedResultException">
        /// Thrown if the result was not properly initialized using a factory method.
        /// </exception>
        public bool Failed => ResultGuard.EnsureInitializedAndReturnFailureState(_failed, _initialized);

        /// <summary>
        /// Gets a value indicating whether the result has succeeded.
        /// </summary>
        /// <remarks>
        /// If the result was not properly initialized, accessing this property will throw
        /// an <see cref="UninitializedResultException"/>.
        /// </remarks>
        /// <exception cref="UninitializedResultException">
        /// Thrown if the result was not properly initialized using a factory method.
        /// </exception>
        public bool Succeeded => !ResultGuard.EnsureInitializedAndReturnFailureState(_failed, _initialized);


        /// <summary>
        /// Creates a successful result with the specified value.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> that represents a successful outcome with the specified value.
        /// </returns>
        public static Result<TError, TValue> Success(TValue value)
        {
            return new Result<TError, TValue>(value);
        }

        /// <summary>
        /// Creates a failed result with the specified error.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> that represents a failed outcome containing the provided error.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="error"/> is <c>null</c>.
        /// </exception>
        public static Result<TError, TValue> Fail(TError error)
        {
            return new Result<TError, TValue>(error);
        }
    }
}

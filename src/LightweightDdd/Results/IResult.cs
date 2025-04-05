// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Results
{
    public interface IResult<out TError>
    {
        /// <summary>
        /// Gets the error associated with the result, if any.
        /// </summary>
        TError Error { get; }

        /// <summary>
        /// Gets a value indicating whether the result represents a successful outcome.
        /// </summary>
        bool Succeeded { get; }

        /// <summary>
        /// Gets a value indicating whether the result represents a failure.
        /// </summary>
        bool Failed { get; }
    }

    public interface IResult<out TError, out TValue> : IResult<TError>
    {
        /// <summary>
        /// Gets the value associated with the result, if any.
        /// </summary>
        TValue Value { get; }
    }
}

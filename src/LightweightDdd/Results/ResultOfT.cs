// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Results
{
    /// <summary>
    /// Represents the outcome of an operation that may either succeed with a value or fail with an error.
    /// </summary>
    /// <typeparam name="TError">The type of the error in case of failure. Must implement <see cref="IError"/>.</typeparam>
    /// <typeparam name="TResult">The type of the value returned when the result is successful.</typeparam>
    public class Result<TError, TResult> : Result<TError>
        where TError : IError
    {
        private readonly TResult _result;

        /// <summary>
        /// Initializes a successful result with the specified value.
        /// </summary>
        /// <param name="value">The value returned by the operation.</param>
        protected internal Result(TResult value)
        {
            _result = value;
        }

        /// <summary>
        /// Initializes a failed result with the specified error.
        /// </summary>
        /// <param name="error">The error associated with the failure.</param>
        protected internal Result(TError error)
            : base(error)
        {
        }

        /// <summary>
        /// Gets the result value if the operation succeeded.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the result represents a failure.</exception>
        public TResult Value => Failed ? throw new InvalidOperationException() : _result;
    }
}

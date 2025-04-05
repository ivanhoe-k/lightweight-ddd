// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace LightweightDdd.Results
{
    /// <summary>
    /// Represents an exception that is thrown when attempting to access a result
    /// that has not been properly initialized.
    /// </summary>
    public sealed class UninitializedResultException : Exception
    {
        public const string DefaultMessage =
            "The result was accessed before being initialized. " +
            "This typically occurs when the result was instantiated using 'default(Result<,>)' or 'new Result()', " +
            "which bypasses the factory methods and violates result consistency and state guarantees.";


        public UninitializedResultException()
            : base(DefaultMessage)
        {
        }

        public UninitializedResultException(string message)
            : base(message)
        {
        }

        public UninitializedResultException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

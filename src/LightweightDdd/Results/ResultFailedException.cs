// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace LightweightDdd.Results
{
    /// <summary>
    /// Thrown when attempting to access <c>Value</c> on a failed result.
    /// </summary>
    public sealed class ResultFailedException : InvalidOperationException
    {
        public const string DefaultMessage = "Cannot access Value because the result has failed.";

        public ResultFailedException()
            : base(DefaultMessage) { }

        public ResultFailedException(string message) : base(message) { }

        public ResultFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}

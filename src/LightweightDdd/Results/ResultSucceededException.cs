// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace LightweightDdd.Results
{
    /// <summary>
    /// Thrown when attempting to access <c>Error</c> on a successful result.
    /// </summary>
    public sealed class ResultSucceededException : InvalidOperationException
    {
        public const string DefaultMessage = "Cannot access Error because the result has succeeded.";

        public ResultSucceededException()
            : base(DefaultMessage) { }

        public ResultSucceededException(string message) : base(message) { }

        public ResultSucceededException(string message, Exception innerException) : base(message, innerException) { }
    }
}

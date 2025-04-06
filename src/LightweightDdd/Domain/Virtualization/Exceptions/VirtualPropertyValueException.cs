// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace LightweightDdd.Domain.Virtualization.Exceptions
{
    /// <summary>
    /// Represents an error that occurs when a virtual property receives an invalid or disallowed value.
    /// </summary>
    public sealed class VirtualPropertyValueException : VirtualPropertyException
    {
        /// <summary>
        /// Initializes a new instance of the exception with a default message based on entity and property names.
        /// </summary>
        public VirtualPropertyValueException(string entityName, string propertyName)
            : base(entityName, propertyName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the exception with a custom message.
        /// </summary>
        public VirtualPropertyValueException(string entityName, string propertyName, string message)
            : base(entityName, propertyName, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the exception with a custom message and an inner exception.
        /// </summary>
        public VirtualPropertyValueException(string entityName, string propertyName, string message, Exception innerException)
            : base(entityName, propertyName, message, innerException)
        {
        }
    }
}

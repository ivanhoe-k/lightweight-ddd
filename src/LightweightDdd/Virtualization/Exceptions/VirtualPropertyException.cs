// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace LightweightDdd.Virtualization.Exceptions
{
    /// <summary>
    /// The base exception for all errors related to virtual properties within the Virtual Entity Pattern.
    /// </summary>
    /// <remarks>
    /// Provides diagnostic context like <see cref="EntityName"/> and <see cref="PropertyName"/>
    /// to help identify misuses or unexpected access to virtual properties during runtime.
    /// </remarks>
    public abstract class VirtualPropertyException : Exception
    {
        /// <summary>
        /// Gets the name of the virtual entity that owns the property.
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// Gets the name of the virtual property involved in the exception.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Initializes a new instance with a default message based on entity and property names.
        /// </summary>
        public VirtualPropertyException(string entityName, string propertyName)
            : base($"An error occurred in virtual property '{propertyName}' of entity '{entityName}'.")
        {
            EntityName = entityName;
            PropertyName = propertyName;
        }

        /// <summary>
        /// Initializes a new instance with a custom error message.
        /// </summary>
        public VirtualPropertyException(string entityName, string propertyName, string message)
            : base(message)
        {
            EntityName = entityName;
            PropertyName = propertyName;
        }

        /// <summary>
        /// Initializes a new instance with a custom message and an inner exception.
        /// </summary>
        public VirtualPropertyException(string entityName, string propertyName, string message, Exception? innerException)
            : base(message, innerException)
        {
            EntityName = entityName;
            PropertyName = propertyName;
        }
    }
}

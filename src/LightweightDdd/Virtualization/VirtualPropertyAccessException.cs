// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Virtualization
{
    /// <summary>
    /// Exception thrown when an unresolved virtual property is accessed.
    /// </summary>
    /// <remarks>
    /// This exception is part of the Virtual Entity Pattern and is used to guard against
    /// unintended access to properties that were not explicitly resolved during partial aggregate hydration.
    ///
    /// It provides clear diagnostic information about the entity and property involved,
    /// making it easier to detect missing initialization during development or testing.
    /// </remarks>
    public sealed class VirtualPropertyAccessException : Exception
    {
        /// <summary>
        /// Gets the name of the property that was accessed without being resolved.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the name of the virtual entity that owns the unresolved property.
        /// </summary>
        public string EntityName { get; }

        public VirtualPropertyAccessException(string propertyName, string entityName)
            : base($"Property '{propertyName}' on virtual entity '{entityName}' was not resolved.")
        {
            PropertyName = propertyName;
            EntityName = entityName;
        }
    }
}

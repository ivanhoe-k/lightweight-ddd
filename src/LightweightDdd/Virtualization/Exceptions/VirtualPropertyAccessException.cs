// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Virtualization.Exceptions
{
    /// <summary>
    /// Exception thrown when an unresolved virtual property is accessed.
    /// </summary>
    /// <remarks>
    /// Used to guard against access to properties that were not explicitly resolved
    /// during selective loading, projection, or partial aggregate hydration.
    /// </remarks>
    public sealed class VirtualPropertyAccessException : VirtualPropertyException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPropertyAccessException"/> class.
        /// </summary>
        /// <param name="propertyName">The unresolved property name.</param>
        /// <param name="entityName">The name of the owning virtual entity.</param>
        public VirtualPropertyAccessException(string propertyName, string entityName)
            : base(entityName, propertyName, $"Property '{propertyName}' on virtual entity '{entityName}' was not resolved.")
        {
        }
    }
}

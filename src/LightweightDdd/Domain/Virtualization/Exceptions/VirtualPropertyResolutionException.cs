// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Domain.Virtualization.Exceptions
{
    /// <summary>
    /// Thrown when a virtual property is resolved more than once.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Virtual properties are designed to be immutable once resolved. Calling <c>Resolve</c>
    /// more than once is considered a misuse of the virtualization infrastructure and indicates a logic error.
    /// </para>
    /// <para>
    /// This exception is typically thrown when infrastructure code attempts to hydrate a virtual property
    /// that has already been resolved. If multiple updates are expected, <c>Update</c> should be used instead
    /// of <c>Resolve</c>.
    /// </para>
    /// </remarks>
    public sealed class VirtualPropertyResolutionException : VirtualPropertyException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPropertyResolutionException"/> class
        /// using the provided entity and property names.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the virtual property being resolved.</param>
        public VirtualPropertyResolutionException(string entityName, string propertyName)
            : base(entityName, propertyName, $"Virtual property '{propertyName}' on entity '{entityName}' has already been resolved.")
        {
        }
    }
}

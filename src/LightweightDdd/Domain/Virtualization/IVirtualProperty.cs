// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Domain.Virtualization
{
    /// <summary>
    ///     Represents a virtual property in a virtual entity that may be partially materialized.
    /// </summary>
    public interface IVirtualProperty
    {
        /// <summary>
        /// Gets the name of the logical property this virtual property represents.
        /// Used for identifying the field in serialization, logging, or update operations.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Gets a value indicating whether this virtual property has been resolved (i.e., loaded or changed through business logic).
        /// </summary>
        bool HasResolved { get; }

        /// <summary>
        /// Gets a value indicating whether the virtual property has been mutated (i.e., its value changed through business logic).
        /// </summary>
        bool HasChanged { get; }
    }
}

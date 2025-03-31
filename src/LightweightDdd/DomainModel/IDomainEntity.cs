// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using LightweightDdd.Events;

namespace LightweightDdd.DomainModel
{
    /// <summary>
    /// Represents a polymorphic contract for a domain entity.
    /// </summary>
    /// <remarks>
    /// This interface provides a non-generic way to work with domain entities,
    /// making it useful for scenarios like persistence, event dispatching, or infrastructure logging
    /// where the specific key type (<c>TKey</c>) is not important.
    ///
    /// The <c>Id</c> is exposed as an <see cref="object"/> to support generality,
    /// and consumers should cast appropriately if strong typing is required.
    ///
    /// The <see cref="DomainEvents"/> collection captures domain-level events
    /// that occurred within the entity and should be processed by the application or infrastructure layer.
    /// </remarks>
    public interface IDomainEntity
    {
        /// <summary>
        /// Gets the unique identifier of the entity.
        /// </summary>
        object Id { get; }

        /// <summary>
        /// Gets the collection of domain events raised by this entity.
        /// </summary>
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    }
}

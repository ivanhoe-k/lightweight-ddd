// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace LightweightDdd.Core.Events
{
    /// <summary>
    /// Base interface for all domain events.
    /// Supports polymorphic dispatch, event tracing, and infrastructure-level processing.
    /// </summary>
    /// <remarks>
    /// This interface provides a general contract for working with domain events
    /// without requiring knowledge of their specific key type.
    ///
    /// The <c>Id</c> represents a unique identifier for the event instance and is exposed
    /// as an <see cref="object"/> to allow flexibility across event types.
    /// For strong typing, use the generic counterpart <c>IDomainEvent&lt;TKey&gt;</c>.
    /// </remarks>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the event.
        /// </summary>
        object EventId { get; }
    }
}

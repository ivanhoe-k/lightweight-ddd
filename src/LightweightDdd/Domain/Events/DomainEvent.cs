// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Domain.Events
{
    /// <summary>
    /// Abstract base class for strongly typed domain events.
    /// </summary>
    /// <typeparam name="TKey">The type of the event identifier.</typeparam>
    /// <remarks>
    /// This class implements <see cref="IDomainEvent"/> explicitly to provide polymorphic access to the event's identity
    /// while retaining strong typing through the generic <c>TKey</c> parameter.
    ///
    /// It ensures that the identifier is non-null and not a default value using <c>ThrowIfNullOrDefault</c> guard logic.
    /// This base is intended to be used by domain events that require both type safety and interoperability
    /// with generic event handling infrastructure (e.g., dispatchers, outbox writers).
    /// </remarks>
    public abstract class DomainEvent<TKey> : IDomainEvent
        where TKey : notnull, IComparable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEvent{TKey}"/> class with the specified identifier.
        /// </summary>
        /// <param name="eventId">The strongly typed identifier of the domain event.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="eventId"/> is null or default.</exception>
        protected DomainEvent(TKey eventId)
        {
            eventId.ThrowIfNullOrDefault();

            EventId = eventId;
        }

        /// <summary>
        /// Gets the strongly typed identifier of the domain event.
        /// </summary>
        public TKey EventId { get; }

        /// <inheritdoc />
        object IDomainEvent.EventId => EventId;
    }
}

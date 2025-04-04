// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using LightweightDdd.Results;

namespace LightweightDdd.Events
{
    /// <summary>
    /// Non-generic domain event handler interface for dynamic dispatching.
    /// This interface is implemented by generic event handlers for unified processing.
    /// </summary>
    public interface IDomainEventHandler
    {
        /// <summary>
        /// Handles a domain event.
        /// </summary>
        /// <param name="event">The domain event to handle.</param>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> containing either an error or a collection of resulting domain effects.
        /// </returns>
        Result<IError, IReadOnlyCollection<IDomainEffect>> Handle(IDomainEvent @event);
    }

    /// <summary>
    /// Represents a handler for a specific type of domain event.
    /// Handlers may return zero or more domain effects, which describe the outcome of the event handling.
    /// </summary>
    /// <typeparam name="TEvent">The specific domain event type handled.</typeparam>
    public interface IDomainEventHandler<TEvent> : IDomainEventHandler
        where TEvent : IDomainEvent
    {
        /// <summary>
        /// Handles a strongly typed domain event.
        /// </summary>
        /// <param name="event">The domain event instance.</param>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> containing either an error or a collection of resulting domain effects.
        /// </returns>
        Result<IError, IReadOnlyCollection<IDomainEffect>> Handle(TEvent @event);
    }
}

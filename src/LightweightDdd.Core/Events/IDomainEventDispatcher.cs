// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using LightweightDdd.Core.Results;

namespace LightweightDdd.Core.Events
{
    /// <summary>
    /// Dispatches one or more domain events to their respective handlers.
    /// This interface represents a coordination mechanism between aggregates and handlers.
    /// </summary>
    public interface IDomainEventDispatcher
    {
        /// <summary>
        /// Dispatches the given domain events to their registered handlers.
        /// </summary>
        /// <param name="events">A collection of domain events to dispatch.</param>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> containing either an error or a collection of resulting domain effects.
        /// </returns>
        Result<IError, IReadOnlyCollection<IDomainEffect>> Dispatch(IReadOnlyCollection<IDomainEvent> events);
    }
}

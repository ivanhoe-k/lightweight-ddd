// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1040 // Avoid empty interfaces
namespace LightweightDdd.Domain.Events
{
    /// <summary>
    /// Marker interface for representing side effects produced by domain logic.
    ///
    /// Domain effects describe operations that need to happen as a result of domain behavior —
    /// such as sending a notification, publishing an event, triggering a job, or performing a related update elsewhere.
    ///
    /// These are typically collected during aggregate execution and handled after persistence,
    /// often by the application layer or infrastructure components.
    ///
    /// In most cases, implementing an outbox pattern is the most reliable and scalable way to persist and dispatch these effects,
    /// ensuring transactional safety and eventual consistency.
    ///
    /// Other implementations (e.g., direct job scheduling or in-process dispatch) may also be appropriate depending on the use case.
    ///
    /// This interface defines intent, not mechanism.
    /// </summary>
    public interface IDomainEffect
    {
    }
}

// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Events;
using LightweightDdd.Extensions;
using System;

namespace LightweightDdd.Examples.Domain.Events.Profile
{
    public abstract class BaseProfileDomainEvent : DomainEvent<Guid>
    {
        public Guid ProfileId { get; }

        protected BaseProfileDomainEvent(Guid eventId, Guid profileId)
            : base(eventId)
        {
            profileId.ThrowIfEmpty();

            ProfileId = profileId;
        }
    }
}

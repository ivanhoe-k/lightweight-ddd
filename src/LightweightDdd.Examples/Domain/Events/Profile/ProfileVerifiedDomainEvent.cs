// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;

namespace LightweightDdd.Examples.Domain.Events.Profile
{
    public sealed class ProfileVerifiedDomainEvent : BaseProfileDomainEvent
    {
        private ProfileVerifiedDomainEvent(Guid eventId, Guid profileId)
            : base(eventId: eventId, profileId: profileId)
        {
        }

        public static ProfileVerifiedDomainEvent Create(Guid profileId)
        {
            return new ProfileVerifiedDomainEvent(
                eventId: Guid.NewGuid(),
                profileId: profileId);
        }
    }

}

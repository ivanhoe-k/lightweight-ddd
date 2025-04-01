// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Events.Profile
{
    public sealed class ProfileOnboardedDomainEvent : BaseProfileDomainEvent
    {
        private ProfileOnboardedDomainEvent(Guid eventId, Guid profileId)
            : base(eventId: eventId, profileId: profileId)
        {
        }

        public static ProfileOnboardedDomainEvent Create(Guid profileId)
        {
            return new ProfileOnboardedDomainEvent(
                eventId: Guid.NewGuid(),
                profileId: profileId);
        }
    }

}

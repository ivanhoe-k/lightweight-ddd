// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Core.Virtualization;
using System;
using System.Collections.Generic;

namespace LightweightDdd.Examples.Domain.Models.Virtualization
{
    public record class VirtualProfileArgs : IVirtualArgs<VirtualProfileArgsBuilder, VirtualProfileArgs>
    {
        private VirtualProfileArgs()
        {
            PersonalInfo = NullableVirtualProperty<Profile, PersonalInfo?>.CreateFor(profile => profile.PersonalInfo);
            Avatar = NullableVirtualProperty<Profile, Media?>.CreateFor(profile => profile.Avatar);
            BackgroundImage = NullableVirtualProperty<Profile, Media?>.CreateFor(profile => profile.BackgroundImage);
            Address = NullableVirtualProperty<Profile, Address?>.CreateFor(profile => profile.Address);
            Subscription = VirtualProperty<Profile, SubscriptionPlan>.CreateFor(profile => profile.Subscription);
            Gallery = VirtualProperty<Profile, IReadOnlyCollection<Media>>.CreateFor(profile => profile.Gallery);
            Verification = VirtualProperty<Profile, VerificationStatus>.CreateFor(profile => profile.Verification);
            IsOnboarded = VirtualProperty<Profile, bool>.CreateFor(profile => profile.IsOnboarded);
        }

        public NullableVirtualProperty<Profile, PersonalInfo?> PersonalInfo { get; init; }

        public NullableVirtualProperty<Profile, Media?> Avatar { get; init; }

        public NullableVirtualProperty<Profile, Media?> BackgroundImage { get; init; }

        public NullableVirtualProperty<Profile, Address?> Address { get; init; }

        public VirtualProperty<Profile, SubscriptionPlan> Subscription { get; init; }

        public VirtualProperty<Profile, IReadOnlyCollection<Media>> Gallery { get; init; }

        public VirtualProperty<Profile, VerificationStatus> Verification { get; init; }

        public VirtualProperty<Profile, bool> IsOnboarded { get; init; }


        public static VirtualProfileArgsBuilder GetBuilder()
        {
            return new VirtualProfileArgsBuilder(new VirtualProfileArgs());
        }
    }
}

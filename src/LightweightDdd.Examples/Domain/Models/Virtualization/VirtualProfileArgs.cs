// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Virtualization;
using System;
using System.Collections.Generic;

namespace LightweightDdd.Examples.Domain.Models.Virtualization
{
    public record class VirtualProfileArgs : IVirtualArgs<VirtualProfileArgsBuilder, VirtualProfileArgs>
    {
        private VirtualProfileArgs()
        {
            PersonalInfo = VirtualProfileProperty<PersonalInfo?>.CreateFor(profile => profile.PersonalInfo);
            Avatar = VirtualProfileProperty<Media?>.CreateFor(profile => profile.Avatar);
            BackgroundImage = VirtualProfileProperty<Media?>.CreateFor(profile => profile.BackgroundImage);
            Address = VirtualProfileProperty<Address?>.CreateFor(profile => profile.Address);
            Gallery = VirtualProfileProperty<IReadOnlyCollection<Media>>.CreateFor(profile => profile.Gallery);
            Verification = VirtualProfileProperty<VerificationStatus>.CreateFor(profile => profile.Verification);
            IsOnboarded = VirtualProfileProperty<bool>.CreateFor(profile => profile.IsOnboarded);
        }

        public VirtualProfileProperty<PersonalInfo?> PersonalInfo { get; init; }

        public VirtualProfileProperty<Media?> Avatar { get; init; }

        public VirtualProfileProperty<Media?> BackgroundImage { get; init; }

        public VirtualProfileProperty<Address?> Address { get; init; }

        public VirtualProfileProperty<IReadOnlyCollection<Media>> Gallery { get; init; }

        public VirtualProfileProperty<VerificationStatus> Verification { get; init; }

        public VirtualProfileProperty<bool> IsOnboarded { get; init; }
            
        public static VirtualProfileArgsBuilder GetBuilder()
        {
            return new VirtualProfileArgsBuilder(new VirtualProfileArgs());
        }
    }
}

// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Extensions;
using System;
using System.Collections.Generic;
using LightweightDdd.Domain.Virtualization;

namespace LightweightDdd.Examples.Domain.Models.Virtualization
{
    public sealed class VirtualProfile : Profile
    {
        private NullableVirtualProperty<Profile, PersonalInfo?> personalInfo;
        private NullableVirtualProperty<Profile, Media?> avatar;
        private NullableVirtualProperty<Profile, Media?> backgroundImage;
        private NullableVirtualProperty<Profile, Address?> address;
        private VirtualProperty<Profile, SubscriptionPlan> subscription;
        private VirtualProperty<Profile, IReadOnlyCollection<Media>> gallery;
        private VirtualProperty<Profile, VerificationStatus> verification;
        private VirtualProperty<Profile, bool> isOnboarded;

        public override PersonalInfo? PersonalInfo
        {
            get => personalInfo.GetValueOrThrow();
            protected set => personalInfo = personalInfo.Update(value);
        }

        public override Media? Avatar
        {
            get => avatar.GetValueOrThrow();
            protected set => avatar = avatar.Update(value);
        }

        public override Media? BackgroundImage
        {
            get => backgroundImage.GetValueOrThrow();
            protected set => backgroundImage = backgroundImage.Update(value);
        }

        public override Address? Address
        {
            get => address.GetValueOrThrow();
            protected set => address = address.Update(value);
        }

        public override SubscriptionPlan Subscription
        {
            get => subscription.GetValueOrThrow();
            protected set => subscription = subscription.Update(value);
        }

        public override IReadOnlyCollection<Media> Gallery
        {
            get => gallery.GetValueOrThrow();
            protected set => gallery = gallery.Update(value);
        }

        public override VerificationStatus Verification
        {
            get => verification.GetValueOrThrow();
            protected set => verification = verification.Update(value);
        }

        public override bool IsOnboarded
        {
            get => isOnboarded.GetValueOrThrow();
            protected set => isOnboarded = isOnboarded.Update(value);
        }

        public VirtualProfile(Guid id, long version, VirtualProfileArgs args) : base(id, version)
        {
            args.ThrowIfNull();

            personalInfo = args.PersonalInfo;
            avatar = args.Avatar;
            backgroundImage = args.BackgroundImage;
            address = args.Address;
            subscription = args.Subscription;
            gallery = args.Gallery;
            verification = args.Verification;
            isOnboarded = args.IsOnboarded;
        }
    }
}

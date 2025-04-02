// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Extensions;
using System;
using System.Collections.Generic;

namespace LightweightDdd.Examples.Domain.Models.Virtualization
{
    public sealed class VirtualProfile : Profile
    {
        private VirtualProfileProperty<PersonalInfo?> personalInfo;
        private VirtualProfileProperty<Media?> avatar;
        private VirtualProfileProperty<Media?> backgroundImage;
        private VirtualProfileProperty<Address?> address;
        private VirtualProfileProperty<SubscriptionPlan> subscription;
        private VirtualProfileProperty<IReadOnlyCollection<Media>> gallery;
        private VirtualProfileProperty<VerificationStatus> verification;
        private VirtualProfileProperty<bool> isOnboarded;

        public override PersonalInfo? PersonalInfo
        {
            get => personalInfo.GetValueOrThrow();
            protected set => personalInfo = personalInfo.Resolve(value);
        }

        public override Media? Avatar
        {
            get => avatar.GetValueOrThrow();
            protected set => avatar = avatar.Resolve(value);
        }

        public override Media? BackgroundImage
        {
            get => backgroundImage.GetValueOrThrow();
            protected set => backgroundImage = backgroundImage.Resolve(value);
        }

        public override Address? Address
        {
            get => address.GetValueOrThrow();
            protected set => address = address.Resolve(value);
        }

        public override SubscriptionPlan Subscription
        {
            get => subscription.GetRequiredValueOrThrow();
            protected set => subscription = subscription.Resolve(value);
        }

        public override IReadOnlyCollection<Media> Gallery
        {
            get => gallery.GetRequiredValueOrThrow();
            protected set => gallery = gallery.Resolve(value);
        }

        public override VerificationStatus Verification
        {
            get => verification.GetRequiredValueOrThrow();
            protected set => verification = verification.Resolve(value);
        }

        public override bool IsOnboarded
        {
            get => isOnboarded.GetRequiredValueOrThrow();
            protected set => isOnboarded = isOnboarded.Resolve(value);
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

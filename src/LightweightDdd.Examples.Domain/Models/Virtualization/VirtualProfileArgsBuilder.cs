// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Virtualization;
using LightweightDdd.Examples.Domain.Models;
using System.Collections.Generic;

namespace LightweightDdd.Examples.Domain.Models.Virtualization
{
    public sealed class VirtualProfileArgsBuilder : VirtualArgsBuilderBase<Profile, VirtualProfileArgs>
    {
        public VirtualProfileArgsBuilder(VirtualProfileArgs args) : base(args)
        {
        }

        public override VirtualProfileArgs Build()
        {
            return Args;
        }

        public VirtualProfileArgsBuilder WithPersonalInfo(PersonalInfo value)
        {
            Args = Args with
            {
                PersonalInfo = ResolveProperty(Args.PersonalInfo, value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithAvatar(Media value)
        {
            Args = Args with
            {
                Avatar = ResolveProperty(Args.Avatar, value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithBackgroundImage(Media value)
        {
            Args = Args with
            {
                BackgroundImage = ResolveProperty(Args.BackgroundImage, value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithGallery(IReadOnlyCollection<Media> value)
        {
            Args = Args with
            {
                Gallery = ResolveProperty(Args.Gallery, value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithAddress(Address value)
        {
            Args = Args with
            {
                Address = ResolveProperty(Args.Address, value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithVerification(VerificationStatus value)
        {
            Args = Args with
            {
                Verification = ResolveProperty(Args.Verification, value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithIsOnboarded(bool value)
        {
            Args = Args with
            {
                IsOnboarded = ResolveProperty(Args.IsOnboarded, value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithSubscription(SubscriptionPlan value)
        {
            Args = Args with
            {
                Subscription = ResolveProperty(Args.Subscription, value)
            };
            return this;
        }
    }

}

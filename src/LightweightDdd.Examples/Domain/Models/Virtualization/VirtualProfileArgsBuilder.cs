// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Virtualization;
using System.Collections.Generic;

namespace LightweightDdd.Examples.Domain.Models.Virtualization
{
    public sealed class VirtualProfileArgsBuilder : IVirtualArgsBuilder<VirtualProfileArgs>
    {
        private VirtualProfileArgs _args;

        public VirtualProfileArgsBuilder(VirtualProfileArgs args)
        {
            _args = args;
        }

        public VirtualProfileArgs Build()
        {
            return _args;
        }

        public VirtualProfileArgsBuilder WithPersonalInfo(PersonalInfo value)
        {
            _args = _args with
            {
                PersonalInfo = _args.PersonalInfo.Resolve(value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithAvatar(Media value)
        {
            _args = _args with
            {
                Avatar = _args.Avatar.Resolve(value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithBackgroundImage(Media value)
        {
            _args = _args with
            {
                BackgroundImage = _args.BackgroundImage.Resolve(value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithGallery(IReadOnlyCollection<Media> value)
        {
            _args = _args with
            {
                Gallery = _args.Gallery.Resolve(value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithAddress(Address value)
        {
            _args = _args with
            {
                Address = _args.Address.Resolve(value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithVerification(VerificationStatus value)
        {
            _args = _args with
            {
                Verification = _args.Verification.Resolve(value)
            };
            return this;
        }

        public VirtualProfileArgsBuilder WithIsOnboarded(bool value)
        {
            _args = _args with
            {
                IsOnboarded = _args.IsOnboarded.Resolve(value)
            };
            return this;
        }
    }

}

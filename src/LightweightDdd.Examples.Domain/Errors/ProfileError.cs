// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Core.Results;

namespace LightweightDdd.Examples.Domain.Errors
{
    public sealed record ProfileError : DomainError<ProfileErrorCode>
    {
        public ProfileError(ProfileErrorCode code)
            : base(code)
        {
        }

        public static IDomainError AlreadyOnboarded() => new ProfileError(ProfileErrorCode.AlreadyOnboarded);

        public static IDomainError AlreadyVerified() => new ProfileError(ProfileErrorCode.AlreadyVerified);

        public static IDomainError InvalidId() => new ProfileError(ProfileErrorCode.InvalidId);

        public static IDomainError InvalidVersion() => new ProfileError(ProfileErrorCode.InvalidVersion);

        public static IDomainError InvalidAddress() => new ProfileError(ProfileErrorCode.InvalidAddress);

        public static IDomainError InvalidPersonalInfo() => new ProfileError(ProfileErrorCode.InvalidPersonalInfo);

        public static IDomainError InvalidGallery() => new ProfileError(ProfileErrorCode.InvalidGallery);
    }
}

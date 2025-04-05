// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Examples.Domain.Errors
{
    public sealed record ProfileError : DomainError<ProfileErrorCode>
    {
        public ProfileError(ProfileErrorCode code)
            : base(code)
        {
        }

        public static IProfileError AlreadyOnboarded() => new ProfileError(ProfileErrorCode.AlreadyOnboarded);

        public static IProfileError AlreadyVerified() => new ProfileError(ProfileErrorCode.AlreadyVerified);

        public static IProfileError InvalidId() => new ProfileError(ProfileErrorCode.InvalidId);

        public static IProfileError InvalidVersion() => new ProfileError(ProfileErrorCode.InvalidVersion);

        public static IProfileError InvalidAddress() => new ProfileError(ProfileErrorCode.InvalidAddress);

        public static IProfileError InvalidPersonalInfo() => new ProfileError(ProfileErrorCode.InvalidPersonalInfo);

        public static IProfileError InvalidGallery() => new ProfileError(ProfileErrorCode.InvalidGallery);
    }
}

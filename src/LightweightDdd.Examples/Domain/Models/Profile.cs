// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.DomainModel;
using LightweightDdd.Events;
using LightweightDdd.Examples.Domain.Errors;
using LightweightDdd.Examples.Domain.Events.Profile;
using LightweightDdd.Results;
using System;
using System.Collections.Generic;

namespace LightweightDdd.Examples.Domain.Models
{
    public sealed class Profile : VersionedDomainEntity<Guid>
    {
        private Profile(Guid id, long version)
            : base(id: id, version: version)
        {
            Gallery = Array.Empty<Media>();
        }

        public static Result<IDomainError, Profile> Create(Guid id, long version)
        {
            if (id == Guid.Empty)
            {
                return Result<IDomainError>.Fail<Profile>(ProfileError.InvalidId());
            }

            return Result<IDomainError>.Ok(new Profile(
                id: id,
                version: version));
        }

        public PersonalInfo? PersonalInfo { get; private set; }

        public Media? Avatar { get; private set; }

        public Media? BackgroundImage { get; private set; }

        public Address? Address { get; private set; }

        public IReadOnlyCollection<Media> Gallery { get; private set; }

        public VerificationStatus Verification { get; private set; } = VerificationStatus.Unverified;

        public bool IsOnboarded => PersonalInfo is not null && Avatar is not null;

        public Result<IDomainError, Profile> CompleteOnboarding(PersonalInfo personalInfo, Media avatar)
        {
            if (IsOnboarded)
            {
                return Result<IDomainError>.Fail<Profile>(ProfileError.AlreadyOnboarded());
            }

            PersonalInfo = personalInfo;
            Avatar = avatar;

            AddDomainEvent(ProfileOnboardedDomainEvent.Create(profileId: Id));

            return Result<IDomainError>.Ok(this);
        }

        public Result<IDomainError, Profile> Verify()
        {
            if (Verification == VerificationStatus.Verified)
            {
                return Result<IDomainError>.Fail<Profile>(ProfileError.AlreadyVerified());
            }

            Verification = VerificationStatus.Verified;

            AddDomainEvent(ProfileVerifiedDomainEvent.Create(profileId: Id));

            return Result<IDomainError>.Ok(this);
        }

        public Result<IDomainError, Profile> UpdateAddress(Address address)
        {
            if (address is null)
            {
                return Result<IDomainError>.Fail<Profile>(ProfileError.InvalidAddress());
            }

            Address = address;
            return Result<IDomainError>.Ok(this);
        }

        public Result<IDomainError, Profile> UpdatePersonalInfo(PersonalInfo info)
        {
            if (info is null)
            {
                return Result<IDomainError>.Fail<Profile>(ProfileError.InvalidPersonalInfo());
            }

            PersonalInfo = info;
            return Result<IDomainError>.Ok(this);
        }

        public Result<IDomainError, Profile> UpdateAvatar(Media? avatar)
        {
            Avatar = avatar;
            AddDomainEvent(ProfileAvatarUpdatedDomainEvent.Create(profileId: Id));

            return Result<IDomainError>.Ok(this);
        }

        public Result<IDomainError, Profile> UpdateBackground(Media? backgroundImage)
        {
            BackgroundImage = backgroundImage;
            AddDomainEvent(ProfileBackgroundImageUpdatedDomainEvent.Create(profileId: Id));

            return Result<IDomainError>.Ok(this);
        }

        public Result<IDomainError, Profile> ReplaceGallery(IReadOnlyCollection<Media> gallery)
        {
            if (gallery is null)
            {
                return Result<IDomainError>.Fail<Profile>(ProfileError.InvalidGallery());
            }

            Gallery = gallery;
            return Result<IDomainError>.Ok(this);
        }
    }

}

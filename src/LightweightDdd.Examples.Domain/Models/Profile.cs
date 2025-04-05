// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Entity;
using LightweightDdd.Domain.Errors;
using LightweightDdd.Domain.Virtualization;
using LightweightDdd.Examples.Domain.Errors;
using LightweightDdd.Examples.Domain.Events.Profile;
using LightweightDdd.Examples.Domain.Models.Virtualization;
using LightweightDdd.Results;
using System;
using System.Collections.Generic;

namespace LightweightDdd.Examples.Domain.Models
{
    public class Profile : VersionedDomainEntity<Guid>, IVirtualizable<VirtualProfile, VirtualProfileArgs, Guid>
    {
        protected Profile(Guid id, long version)
            : base(id: id, version: version)
        {
        }

        public static Result<IDomainError, Profile> Create(Guid id, long version)
        {
            if (id == Guid.Empty)
            {
                return Result<IDomainError>.Fail<Profile>(ProfileError.InvalidId());
            }

            return Result<IDomainError>.Success(new Profile(
                id: id,
                version: version));
        }

        public static Result<IDomainError, VirtualProfile> CreateVirtual(Guid id, long version, VirtualProfileArgs args)
        {
            if (id == Guid.Empty)
            {
                return Result<IDomainError>.Fail<VirtualProfile>(ProfileError.InvalidId());
            }

            return Result<IDomainError>.Success(new VirtualProfile(id, version, args));
        }

        public virtual PersonalInfo? PersonalInfo { get; protected set; }

        public virtual Media? Avatar { get; protected set; }

        public virtual Media? BackgroundImage { get; protected set; }

        public virtual Address? Address { get; protected set; }

        public virtual SubscriptionPlan Subscription { get; protected set; }

        public virtual IReadOnlyCollection<Media> Gallery { get; protected set; }

        public virtual VerificationStatus Verification { get; protected set; }

        public virtual bool IsOnboarded { get; protected set; }

        public Result<IProfileError, Profile> CompleteOnboarding(PersonalInfo personalInfo, Media avatar)
        {
            if (IsOnboarded)
            {
                return Result<IProfileError>.Fail<Profile>(ProfileError.AlreadyOnboarded());
            }

            PersonalInfo = personalInfo;
            Avatar = avatar;
            IsOnboarded = true;

            AddDomainEvent(ProfileOnboardedDomainEvent.Create(profileId: Id));

            return Result<IProfileError>.Success(this);
        }

        public Result<IProfileError, Profile> Verify()
        {
            if (Verification == VerificationStatus.Verified)
            {
                return Result<IProfileError>.Fail<Profile>(ProfileError.AlreadyVerified());
            }

            Verification = VerificationStatus.Verified;

            AddDomainEvent(ProfileVerifiedDomainEvent.Create(profileId: Id));

            return Result<IProfileError>.Success(this);
        }

        public Result<IProfileError, Profile> UpdateAddress(Address address)
        {
            if (address is null)
            {
                return Result<IProfileError>.Fail<Profile>(ProfileError.InvalidAddress());
            }

            Address = address;
            return Result<IProfileError>.Success(this);
        }

        public Result<IProfileError, Profile> UpdatePersonalInfo(PersonalInfo info)
        {
            if (info is null)
            {
                return Result<IProfileError>.Fail<Profile>(ProfileError.InvalidPersonalInfo());
            }

            PersonalInfo = info;
            return Result<IProfileError>.Success(this);
        }

        public Result<IProfileError, Profile> UpdateAvatar(Media? avatar)
        {
            Avatar = avatar;
            AddDomainEvent(ProfileAvatarUpdatedDomainEvent.Create(profileId: Id));

            return Result<IProfileError>.Success(this);
        }

        public Result<IProfileError, Profile> UpdateBackground(Media? backgroundImage)
        {
            BackgroundImage = backgroundImage;
            AddDomainEvent(ProfileBackgroundImageUpdatedDomainEvent.Create(profileId: Id));

            return Result<IProfileError>.Success(this);
        }

        public Result<IProfileError, Profile> UpdateGallery(IReadOnlyCollection<Media> gallery)
        {
            if (gallery is null)
            {
                return Result<IProfileError>.Fail<Profile>(GalleryError.GalleryNotProvided());
            }

            if (gallery.Count > Subscription.MaxGalleryImages)
            {
                return Result<IProfileError>.Fail<Profile>(GalleryError.ExceedsImageLimit());
            }

            Gallery = gallery;
            return Result<IProfileError>.Success(this);
        }
    }

}

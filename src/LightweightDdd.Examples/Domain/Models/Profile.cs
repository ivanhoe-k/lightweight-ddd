// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.DomainModel;
using LightweightDdd.Events;
using LightweightDdd.Examples.Domain.Errors;
using LightweightDdd.Examples.Domain.Events.Profile;
using LightweightDdd.Examples.Domain.Models.Virtualization;
using LightweightDdd.Results;
using LightweightDdd.Virtualization;
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

            return Result<IDomainError>.Ok(new Profile(
                id: id,
                version: version));
        }

        public static Result<IError, VirtualProfile> CreateVirtual(Guid id, long version, VirtualProfileArgs args)
        {
            if (id == Guid.Empty)
            {
                return Result<IError>.Fail<VirtualProfile>(ProfileError.InvalidId());
            }

            return Result<IError>.Ok(new VirtualProfile(id, version, args));
        }

        public virtual PersonalInfo? PersonalInfo { get; protected set; }

        public virtual Media? Avatar { get; protected set; }

        public virtual Media? BackgroundImage { get; protected set; }

        public virtual Address? Address { get; protected set; }

        public virtual SubscriptionPlan Subscription { get; protected set; }

        public virtual IReadOnlyCollection<Media> Gallery { get; protected set; }

        public virtual VerificationStatus Verification { get; protected set; }

        public virtual bool IsOnboarded { get; protected set; }

        public Result<IDomainError, Profile> CompleteOnboarding(PersonalInfo personalInfo, Media avatar)
        {
            if (IsOnboarded)
            {
                return Result<IDomainError>.Fail<Profile>(ProfileError.AlreadyOnboarded());
            }

            PersonalInfo = personalInfo;
            Avatar = avatar;
            IsOnboarded = true;

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

        public Result<IDomainError, Profile> UpdateGallery(IReadOnlyCollection<Media> gallery)
        {
            if (gallery is null)
            {
                return Result<IDomainError>.Fail<Profile>(GalleryError.GalleryNotProvided());
            }

            if (gallery.Count > Subscription.MaxGalleryImages)
            {
                return Result<IDomainError>.Fail<Profile>(GalleryError.ExceedsImageLimit());
            }

            Gallery = gallery;
            return Result<IDomainError>.Ok(this);
        }
    }

}

﻿// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Examples.Domain.Errors
{
    public enum ProfileErrorCode
    {
        AlreadyOnboarded,
        AlreadyVerified,
        InvalidId,
        InvalidVersion,
        InvalidAddress,
        InvalidPersonalInfo,
        InvalidAvatar,
        InvalidBackgroundImage,
        InvalidGallery,
    }
}

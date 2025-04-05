// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Errors
{
    public sealed record GalleryError : DomainError<GalleryErrorCode>
    {
        private GalleryError(GalleryErrorCode code) : base(code)
        {
        }

        public static IProfileError GalleryNotProvided() =>
            new GalleryError(GalleryErrorCode.GalleryNotProvided);

        public static IProfileError ExceedsImageLimit() =>
            new GalleryError(GalleryErrorCode.ExceedsImageLimit);
    }
}

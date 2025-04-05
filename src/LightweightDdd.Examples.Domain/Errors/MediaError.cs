// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Examples.Domain.Errors
{
    public sealed record MediaError : DomainError<MediaErrorCode>
    {
        private MediaError(MediaErrorCode original) 
            : base(original)
        {
        }

        public static IProfileError MissingUrl() => new MediaError(MediaErrorCode.MissingUrl);

        public static IProfileError InvalidUrl() => new MediaError(MediaErrorCode.InvalidUrl);

        public static IProfileError MissingFileName() => new MediaError(MediaErrorCode.MissingFileName);

        public static IProfileError MissingMimeType() => new MediaError(MediaErrorCode.MissingMimeType);
    }
}

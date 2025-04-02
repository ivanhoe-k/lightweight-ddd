// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Errors
{
    public sealed record MediaError : DomainError<MediaErrorCode>
    {
        private MediaError(MediaErrorCode original) 
            : base(original)
        {
        }

        public static IDomainError MissingUrl() => new MediaError(MediaErrorCode.MissingUrl);

        public static IDomainError InvalidUrl() => new MediaError(MediaErrorCode.InvalidUrl);

        public static IDomainError MissingFileName() => new MediaError(MediaErrorCode.MissingFileName);

        public static IDomainError MissingMimeType() => new MediaError(MediaErrorCode.MissingMimeType);
    }
}

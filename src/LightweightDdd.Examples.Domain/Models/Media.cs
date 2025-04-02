// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Errors;
using LightweightDdd.Core.Results;
using System;

namespace LightweightDdd.Examples.Domain.Models
{
    public sealed record Media
    {
        private Media(string url, string fileName, string mimeType)
        {
            Url = url;
            FileName = fileName;
            MimeType = mimeType;
        }

        public string Url { get; }

        public string FileName { get; }

        public string MimeType { get; }

        public static Result<IDomainError, Media> Create(string url, string fileName, string mimeType)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return Result<IDomainError>.Fail<Media>(MediaError.MissingUrl());
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return Result<IDomainError>.Fail<Media>(MediaError.InvalidUrl());
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Result<IDomainError>.Fail<Media>(MediaError.MissingFileName());
            }

            if (string.IsNullOrWhiteSpace(mimeType))
            {
                return Result<IDomainError>.Fail<Media>(MediaError.MissingMimeType());
            }

            return Result<IDomainError>.Ok(new Media(
                url: url.Trim(),
                fileName: fileName.Trim(),
                mimeType: mimeType.Trim()));
        }
    }

}

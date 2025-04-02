// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Contracts
{
    /// <summary>
    /// Write-only repository for persisting <see cref="Profile"/> aggregates after domain updates.
    /// 
    /// This interface is part of a domain-oriented CQRS approach, where mutation logic is isolated for better control,
    /// auditability, and intent-driven design. It avoids accidental querying and encourages clear write flows.
    /// 
    /// Like its read counterpart, this is defined at the domain layer — abstracted away from infrastructure details.
    /// </summary>
    public interface IProfileWriteOnlyRepository
    {
        /// <summary>
        /// Updates the gallery for a given profile in the storage layer.
        /// The full domain logic should already have been validated before calling this method.
        /// </summary>
        Task<Result<IDomainError>> UpdateGalleryAsync(
            Profile profile,
            CancellationToken cancellationToken);
    }
}

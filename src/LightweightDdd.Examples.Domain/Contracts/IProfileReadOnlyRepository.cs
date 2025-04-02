// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Core.Results;
using LightweightDdd.Examples.Domain.Models.Virtualization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Contracts
{
    /// <summary>
    /// Represents a repository abstraction responsible for fetching and resolving Profile aggregates.
    /// 
    /// This interface includes methods that support partial hydration using the Virtual Entity Pattern,
    /// allowing only the necessary parts of the aggregate to be loaded depending on the specific business use case.
    /// </summary>
    public interface IProfileReadOnlyRepository
    {
        /// <summary>
        /// Resolves a profile for gallery update by partially hydrating only the fields required for this operation.
        /// This avoids loading the full aggregate, enabling more efficient data access and bounded business logic.
        /// </summary>
        Task<Result<IDomainError, VirtualProfile>> ResolveForGalleryUpdateAsync(
            Guid profileId,
            CancellationToken cancellationToken);
    }
}

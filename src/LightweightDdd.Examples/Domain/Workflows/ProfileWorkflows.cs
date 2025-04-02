// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Contracts;
using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Extensions;
using LightweightDdd.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Workflows
{
    /// <summary>
    /// Orchestrates domain logic for <see cref="Profile"/> entities by coordinating business flows
    /// across aggregates, value objects, and domain-level contracts (ports).
    ///
    /// This class represents a domain-level workflow — a dedicated orchestrator responsible for:
    /// - Hydrating aggregates via domain ports
    /// - Executing domain methods and enforcing business invariants
    /// - Coordinating related components in response to a specific use case
    ///
    /// 🧭 Workflows act as use-case handlers within the domain layer.
    /// They are:
    /// - Technology-agnostic
    /// - Testable in isolation
    /// - Focused on orchestrating domain components and rules
    ///
    /// Workflows communicate with external systems exclusively via domain-defined ports,
    /// such as repositories, notification senders, or other abstractions.
    /// </summary>
    public sealed class ProfileWorkflows : IProfileWorkflows
    {
        private readonly ILogger<ProfileWorkflows> _logger;
        private readonly IProfileReadOnlyRepository _readOnlyRepository;
        private readonly IProfileWriteOnlyRepository _writeOnlyRepository;

        public ProfileWorkflows(
            ILogger<ProfileWorkflows> logger,
            IProfileReadOnlyRepository readOnlyRepository,
            IProfileWriteOnlyRepository writeOnlyRepository)
        {
            logger.ThrowIfNull();
            readOnlyRepository.ThrowIfNull();
            writeOnlyRepository.ThrowIfNull();

            _logger = logger;
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
        }

        /// <summary>
        /// Updates a profile's gallery, enforcing domain rules such as subscription limits or validation.
        /// This method performs:
        /// - Partial read via virtual entity
        /// - Aggregate mutation through domain methods
        /// - Persisting changes using the write-only repository
        /// </summary>
        public async Task<Result<IDomainError, IReadOnlyCollection<Media>>> UpdateGalleryAsync(
            Guid profileId,
            IReadOnlyCollection<Media> newGallery,
            CancellationToken cancellationToken)
        {
            profileId.ThrowIfEmpty();
            newGallery.ThrowIfNull();

            _logger.LogDebug("Resolving virtual profile for gallery update (ID: {ProfileId})", profileId);

            var resolveResult = await _readOnlyRepository.ResolveForGalleryUpdateAsync(profileId, cancellationToken);

            if (resolveResult.Failed)
            {
                return Result<IDomainError>.Fail<IReadOnlyCollection<Media>>(resolveResult.Error!);
            }

            var virtualProfile = resolveResult.Value;
            var updateResult = virtualProfile.UpdateGallery(newGallery);

            if (updateResult.Failed)
            {
                return Result<IDomainError>.Fail<IReadOnlyCollection<Media>>(resolveResult.Error!);
            }

            var writeResult = await _writeOnlyRepository.UpdateGalleryAsync(virtualProfile, cancellationToken);

            if (writeResult.Failed)
            {
                return Result<IDomainError>.Fail<IReadOnlyCollection<Media>>(writeResult.Error!);
            }

            return Result<IDomainError>.Ok(virtualProfile.Gallery);
        }
    }
}

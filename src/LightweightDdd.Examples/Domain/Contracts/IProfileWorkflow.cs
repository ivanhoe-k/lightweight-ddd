// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Results;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Contracts
{
    /// <summary>
    /// Defines high-level business workflows for the Profile aggregate.
    ///
    /// A workflow represents a domain-level use case — an orchestrator that coordinates
    /// aggregate methods, value objects, domain events, and other domain concerns.
    ///
    /// Key Principle:
    /// Workflows should rely only on domain-level contracts (Ports) — such as repositories,
    /// domain services, factories, and other abstractions defined inside the domain boundary.
    /// They should never directly depend on infrastructure, external tools, or anything outside
    /// of the domain layer.
    ///
    /// This makes workflows consistent, technology-agnostic, and highly testable — acting as
    /// application entry points that remain pure and focused on domain logic.
    ///
    /// ✅ Workflows call and compose domain behavior.  
    /// ❌ Workflows do not perform persistence, mapping, or external communication themselves.
    ///
    /// 👉 In larger systems, related use cases can be grouped into a single interface (e.g., `IProfileWorkflows`)
    /// instead of creating one class per workflow to reduce boilerplate.
    /// </summary>
    public interface IProfileWorkflows
    {
        /// <summary>
        /// Replaces the gallery images for a given profile by partially hydrating only the fields required for this operation.
        /// </summary>
        Task<Result<IDomainError, IReadOnlyCollection<Media>>> UpdateGalleryAsync(Guid profileId, IReadOnlyCollection<Media> gallery, CancellationToken cancellationToken);
    }
}
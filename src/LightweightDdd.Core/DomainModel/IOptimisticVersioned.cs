// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Core.DomainModel
{
    /// <summary>
    /// Represents a domain object that participates in optimistic concurrency control
    /// by exposing a version number used to detect conflicting updates.
    /// </summary>
    public interface IOptimisticVersioned
    {
        /// <summary>
        /// Gets the version number used for optimistic concurrency checks.
        /// </summary>
        long Version { get; }
    }
}

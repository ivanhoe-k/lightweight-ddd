// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using LightweightDdd.Events;
using LightweightDdd.Extensions;

namespace LightweightDdd.DomainModel
{
    /// <summary>
    /// Abstract base class for domain entities that support optimistic concurrency control using a version number.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
    public abstract class VersionedDomainEntity<TKey> : DomainEntity<TKey>, IOptimisticVersioned
        where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionedDomainEntity{TKey}"/> class
        /// with the specified identifier and version.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="version">The version used for optimistic concurrency control.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="version"/> is less than zero.</exception>
        public VersionedDomainEntity(TKey id, long version)
            : base(id)
        {
            version.ThrowIf(version < 0, "Version must be greater or equal to zero.");

            Version = version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionedDomainEntity{TKey}"/> class
        /// with the specified identifier, version, and domain events.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="version">The version used for optimistic concurrency control.</param>
        /// <param name="domainEvents">The collection of domain events associated with the entity.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="version"/> is less than zero.</exception>
        protected VersionedDomainEntity(TKey id, long version, IReadOnlyCollection<IDomainEvent> domainEvents)
            : base(id, domainEvents)
        {
            version.ThrowIf(version < 0, "Version must be greater or equal to zero.");

            Version = version;
        }

        /// <summary>
        /// Gets the version number used for optimistic concurrency control.
        /// </summary>
        public long Version { get; }
    }
}

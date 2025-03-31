// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using LightweightDdd.Events;
using LightweightDdd.Extensions;
using LightweightDdd.Results;

namespace LightweightDdd.DomainModel
{
    /// <summary>
    /// Abstract base class for domain entities with a strongly typed identifier.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
    public abstract class DomainEntity<TKey>
        : IDomainEntity, IEquatable<DomainEntity<TKey>>, IComparable<DomainEntity<TKey>>, IComparable
        where TKey : notnull, IComparable<TKey>
    {
        private readonly List<IDomainEvent> _domainEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEntity{TKey}"/> class with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id"/> is null or the default value.</exception>
        protected DomainEntity(TKey id)
        {
            id.ThrowIfNullOrDefault();

            Id = id;

            _domainEvents = new List<IDomainEvent>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEntity{TKey}"/> class with the specified identifier and domain events.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="domainEvents">The collection of domain events associated with the entity.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="id"/> is null or the default value.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="domainEvents"/> is null.</exception>
        protected DomainEntity(TKey id, IReadOnlyCollection<IDomainEvent> domainEvents)
            : this(id)
        {
            domainEvents.ThrowIfNull();

            _domainEvents = [.. domainEvents];
        }

        /// <summary>
        /// Gets the strongly typed identifier of the entity.
        /// </summary>
        public TKey Id { get; }

        /// <summary>
        /// Gets the polymorphic identifier of the entity.
        /// </summary>
        object IDomainEntity.Id => Id;

        /// <summary>
        /// Gets the collection of domain events raised by the entity.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnlyCollection();

        public bool Equals(DomainEntity<TKey>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id!.Equals(other.Id);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DomainEntity<TKey>);
        }

        public override int GetHashCode()
        {
            return Id!.GetHashCode();
        }

        public int CompareTo(DomainEntity<TKey>? other)
        {
            if (other is null)
            {
                return 1;
            }

            return Id!.CompareTo(other.Id);
        }

        public int CompareTo(object? obj)
        {
            return CompareTo(obj as DomainEntity<TKey>);
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            domainEvent.ThrowIfNull();

            _domainEvents.Add(domainEvent);
        }
    }
}

// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using LightweightDdd.DomainModel;
using LightweightDdd.Results;

namespace LightweightDdd.Virtualization
{
    /// <summary>
    /// Represents an entity that can be virtualized — i.e., initialized in a partial state
    /// without loading the full underlying model from the data store.
    /// </summary>
    /// <typeparam name="TVirtual">
    /// The type of the virtualized entity, which must inherit from <see cref="DomainEntity{TKey}"/>.
    /// </typeparam>
    /// <typeparam name="TVirtualArgs">
    /// The type of the input arguments used to initialize the virtual entity. This is typically a
    /// record or DTO (e.g., <c>VirtualProfileArgs</c>) encapsulating optional fields for partial construction.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of the entity identifier (e.g., <see cref="Guid"/>, <see cref="int"/>).
    /// </typeparam>
    public interface IVirtualizable<TVirtual, TVirtualArgs, TKey>
        where TVirtual : DomainEntity<TKey>
        where TVirtualArgs : class
        where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Creates a virtualized version of the entity using the specified identifier, version,
        /// and virtual argument object.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="version">The version of the entity, used for concurrency control or tracking.</param>
        /// <param name="args">
        /// A strongly-typed argument object used to partially initialize the virtual entity. This object
        /// should contain all optional data needed for the virtual instance.
        /// </param>
        /// <returns>
        /// A <see cref="Result{TError, TValue}"/> representing either a successfully constructed
        /// virtual entity or a <see cref="IError"/> indicating the reason for failure.
        /// </returns>
        static abstract Result<IError, TVirtual> CreateVirtual(Guid id, long version, TVirtualArgs args);
    }
}

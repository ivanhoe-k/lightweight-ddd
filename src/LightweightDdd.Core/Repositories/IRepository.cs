// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Core.DomainModel;

#pragma warning disable CA1040 // Avoid empty interfaces
namespace LightweightDdd.Core.Repositories
{
    /// <summary>
    /// Optional marker interface for aggregate repositories.
    /// Used to indicate that a repository works with a specific domain entity type.
    /// Does not enforce any CRUD contract or implementation pattern.
    /// </summary>
    /// <typeparam name="TEntity">The domain entity type managed by the repository.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : IDomainEntity
    {
    }
}

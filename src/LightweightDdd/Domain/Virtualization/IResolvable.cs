// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Entity;
using LightweightDdd.Domain.Virtualization.Exceptions;

namespace LightweightDdd.Domain.Virtualization
{
    /// <summary>
    /// Infrastructure-only interface used exclusively by <see cref="VirtualArgsBuilderBase{TEntity, TArgs}"/>
    /// to perform one-time hydration of virtual properties during the construction of virtual entities.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface supports the hydration phase of the Virtual Entity Pattern by providing a safe, restricted way
    /// to inject values into virtual properties via the <see cref="Resolve"/> method.
    /// </para>
    /// <para>
    /// This interface should never be used for business-level mutation. Domain logic should rely on <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Update"/>
    /// to modify virtual properties as part of use case flows.
    /// </para>
    /// </remarks>
    /// <typeparam name="TEntity">The domain entity that owns the virtual property.</typeparam>
    /// <typeparam name="TProperty">The type of the value being resolved into the property.</typeparam>
    /// <typeparam name="TVirtual">
    /// The concrete virtual property type that wraps <typeparamref name="TProperty"/>.
    /// Must derive from <see cref="VirtualPropertyBase{TEntity, TProperty, TVirtual}"/>.
    /// </typeparam>
    public interface IResolvable<TEntity, TProperty, TVirtual>
        where TEntity : IDomainEntity
        where TVirtual : VirtualPropertyBase<TEntity, TProperty, TVirtual>
    {
        /// <summary>
        /// Resolves the virtual property with the specified value during hydration.
        /// </summary>
        /// <remarks>
        /// This method is intended to be called only once per property during the hydration phase.
        /// Repeated calls will throw a <see cref="VirtualPropertyResolutionException"/>.
        /// Use <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Update"/> instead when applying business-level changes after construction.
        /// </remarks>
        /// <param name="value">The value to hydrate into the virtual property.</param>
        /// <returns>A new resolved instance of the virtual property.</returns>
        TVirtual Resolve(TProperty value);
    }
}

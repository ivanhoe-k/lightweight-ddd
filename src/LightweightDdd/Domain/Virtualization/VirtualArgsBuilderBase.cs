// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Entity;
using LightweightDdd.Domain.Virtualization.Exceptions;
using LightweightDdd.Extensions;
using System;

namespace LightweightDdd.Domain.Virtualization
{
    /// <summary>
    /// Abstract base class for defining type-safe builders that construct <see cref="IVirtualArgs"/> instances
    /// used to initialize virtual entities (e.g., <c>VirtualEntity</c>) that wrap a specific domain entity.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The domain entity type for which virtualization is applied.
    /// While <typeparamref name="TEntity"/> itself does not directly own virtual properties,
    /// its virtual counterpart (e.g., <c>VirtualEntity</c>) does.
    /// </typeparam>
    /// <typeparam name="TArgs">
    /// The virtual argument structure used to initialize the virtual entity. Must implement <see cref="IVirtualArgs"/>.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// Builders derived from this base class are designed to safely hydrate virtual arguments —
    /// containers of deferred, partially loaded data used to initialize virtual entities.
    /// </para>
    /// <para>
    /// These builders are infrastructure-level components and should typically be used inside
    /// repositories, projections, or factories responsible for virtual entity construction.
    /// They are <b>not</b> intended for use in domain logic or application services.
    /// </para>
    /// </remarks>
    public abstract class VirtualArgsBuilderBase<TEntity, TArgs>
        where TEntity : IDomainEntity
        where TArgs : IVirtualArgs
    {
        /// <summary>
        /// The virtual argument instance being built.
        /// </summary>
        protected TArgs Args { get; set; }

        /// <summary>
        /// Initializes a new instance of the builder with the specified virtual argument instance.
        /// </summary>
        /// <param name="args">The virtual argument instance to build.</param>
        protected VirtualArgsBuilderBase(TArgs args)
        {
            args.ThrowIfNull();

            Args = args ;
        }

        /// <summary>
        /// Finalizes and returns the configured virtual argument instance.
        /// This method is expected to be implemented by subclasses to complete the construction 
        /// and provide the final virtual argument instance.
        /// </summary>
        /// <returns>The fully constructed virtual argument instance of type <typeparamref name="TArgs"/>.</returns>
        public abstract TArgs Build();

        /// <summary>
        /// Performs one-time hydration of a virtual property using the provided value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is intended for use within <see cref="VirtualArgsBuilderBase{TEntity, TArgs}"/> implementations only,
        /// where it safely hydrates unresolved virtual properties during the construction of virtual entities.
        /// </para>
        /// <para>
        /// Internally, it calls the <see cref="IResolvable{TEntity, TProperty, TVirtual}.Resolve"/> method,
        /// which enforces single-use semantics. Calling it on an already resolved property will result in a
        /// <see cref="VirtualPropertyResolutionException"/>.
        /// </para>
        /// <para>
        /// This method is <b>not</b> intended for domain-level mutation. Use <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Update"/> instead to apply
        /// business changes after initialization.
        /// </para>
        /// </remarks>
        /// <typeparam name="TProperty">The type of the value being hydrated into the virtual property.</typeparam>
        /// <typeparam name="TVirtual">The concrete virtual property type that wraps <typeparamref name="TProperty"/>.</typeparam>
        /// <param name="resolvable">The resolvable instance representing the unresolved virtual property.</param>
        /// <param name="value">The value to hydrate into the virtual property.</param>
        /// <returns>A new resolved instance of the virtual property.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="resolvable"/> is <c>null</c>.
        /// </exception>
        protected TVirtual ResolveProperty<TProperty, TVirtual>(IResolvable<TEntity, TProperty, TVirtual> resolvable, TProperty value)
            where TVirtual : VirtualPropertyBase<TEntity, TProperty, TVirtual>
        {
            resolvable.ThrowIfNull();

            return resolvable.Resolve(value);
        }
    }
}

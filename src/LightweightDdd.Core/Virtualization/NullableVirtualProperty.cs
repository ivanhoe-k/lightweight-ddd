// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Core.DomainModel;
using LightweightDdd.Core.Extensions;
using LightweightDdd.Core.Virtualization.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Core.Virtualization
{
    /// <summary>
    /// Represents a virtual property that allows the resolved value to be null.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the nullable specialization of <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}"/>.
    /// It supports virtualizing properties that may have a <c>null</c> value when resolved, such as reference types,
    /// nullable value types, or optional domain fields.
    /// </para>
    ///
    /// <para>
    /// Consumers are expected to define concrete sealed leaf types such as:
    /// <c>VirtualUserProperty{T}</c> or similar, using this class as a base when the resolved value may be null.
    /// </para>
    /// </remarks>
    /// <typeparam name="TEntity">The domain entity type this virtual property is defined on.</typeparam>
    /// <typeparam name="TProperty">The underlying type of the resolved value. May be nullable.</typeparam>
    /// <typeparam name="TSelf">
    /// The self-referencing type used for CRTP-style fluent factory methods. Must be a subclass of this type.
    /// </typeparam>
    public abstract record NullableVirtualProperty<TEntity, TProperty, TSelf> : VirtualPropertyBase<TEntity, TProperty?, TSelf>
         where TEntity : IDomainEntity
         where TSelf : NullableVirtualProperty<TEntity, TProperty, TSelf>
    {
        /// <summary>
        /// Initializes a new unresolved instance of the virtual property using a strongly typed expression.
        /// </summary>
        /// <param name="propertyExp">The lambda expression representing the property being virtualized.</param>
        protected NullableVirtualProperty(Expression<Func<TEntity, TProperty?>> propertyExp) 
            : base(propertyExp) 
        { 
        }

        /// <summary>
        /// Initializes a resolved instance of the virtual property with a nullable value.
        /// </summary>
        /// <param name="entity">The name of the owning entity.</param>
        /// <param name="property">The name of the property.</param>
        /// <param name="value">The resolved value. May be null.</param>
        protected NullableVirtualProperty(string entity, string property, TProperty? value) 
            : base(entity, property, value)
        { 
        }

        /// <summary>
        /// Returns the resolved value of the virtual property, which may be null.
        /// </summary>
        /// <returns>The resolved value or null.</returns>
        /// <exception cref="VirtualPropertyAccessException">Thrown if the property is not resolved.</exception>
        public TProperty? GetValueOrThrow() => InternalGetValueOrThrow();
    }

    /// <summary>
    /// Represents a sealed, nullable virtual property that does not require subclassing.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a built-in sealed variant of <see cref="NullableVirtualProperty{TEntity, TProperty, TSelf}"/>
    /// that simplifies usage for cases where defining a dedicated subclass would be excessive.
    /// </para>
    ///
    /// <para>
    /// Internally, this type relies on reflection to construct instances, even though its constructors are <c>private</c>.
    /// This is enabled via reflection and constructor metadata caching inside the virtualization infrastructure.
    /// </para>
    ///
    /// <para>
    /// This type must be instantiated via
    /// <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Unresolved"/> and
    /// <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Resolve"/> factory methods only.
    /// Public constructors are intentionally not exposed.
    /// </para>
    /// </remarks>
    /// <typeparam name="TEntity">The entity type that owns the property.</typeparam>
    /// <typeparam name="TProperty">The nullable value type of the virtual property.</typeparam>
    public sealed record NullableVirtualProperty<TEntity, TProperty> : NullableVirtualProperty<TEntity, TProperty, NullableVirtualProperty<TEntity, TProperty>>
        where TEntity : IDomainEntity
    {
        /// <summary>
        /// Private constructor for use by reflection during unresolved factory creation.
        /// </summary>
        /// <param name="expression">A lambda pointing to the unresolved property.</param>
        private NullableVirtualProperty(Expression<Func<TEntity, TProperty?>> expression) 
            : base(expression) 
        {
        }

        /// <summary>
        /// Private constructor for use by reflection during resolution with a known value.
        /// </summary>
        /// <param name="entity">The owning entity name.</param>
        /// <param name="property">The property name.</param>
        /// <param name="value">The nullable resolved value.</param>
        private NullableVirtualProperty(string entity, string property, TProperty? value) 
            : base(entity, property, value) 
        {
        }
    }
}

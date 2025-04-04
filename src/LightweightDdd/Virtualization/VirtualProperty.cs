// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using LightweightDdd.DomainModel;
using LightweightDdd.Extensions;
using LightweightDdd.Utilities;
using LightweightDdd.Virtualization.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LightweightDdd.Virtualization
{
    /// <summary>
    /// Represents a virtual property that enforces non-nullability at the type level.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the non-nullable specialization of <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}"/>. 
    /// It guarantees at runtime that the resolved value is never <c>null</c>.
    /// </para>
    ///
    /// <para>
    /// The <typeparamref name="TProperty"/> is assumed to be non-nullable. If <c>null</c> is passed into the constructor,
    /// an <see cref="ArgumentNullException"/> is thrown defensively at construction time.
    /// </para>
    ///
    /// <para>
    /// Consumers are expected to define concrete sealed leaf types such as:
    /// <c>VirtualUserProperty{T}</c> or similar, using this class as a base.
    /// </para>
    /// </remarks>
    /// <typeparam name="TEntity">The domain entity type this virtual property is defined on.</typeparam>
    /// <typeparam name="TProperty">The type of the resolved value. Must be non-nullable.</typeparam>
    /// <typeparam name="TSelf">
    /// The self-referencing type used for CRTP-style fluent factory methods. Must be a subclass of this type.
    /// </typeparam>
    public abstract record VirtualProperty<TEntity, TProperty, TSelf> : VirtualPropertyBase<TEntity, TProperty, TSelf>
        where TEntity : IDomainEntity
        where TSelf : VirtualProperty<TEntity, TProperty, TSelf>
        where TProperty: notnull
    {
        /// <summary>
        /// Initializes a new unresolved instance of the virtual property using a strongly typed expression.
        /// </summary>
        /// <param name="propertyExp">The lambda expression representing the property being virtualized.</param>
        protected VirtualProperty(Expression<Func<TEntity, TProperty>> propertyExp)
            : base(propertyExp) 
        { 
        }

        /// <summary>
        /// Initializes a resolved instance of the virtual property with a required value.
        /// </summary>
        /// <param name="entity">The name of the owning entity.</param>
        /// <param name="property">The name of the property.</param>
        /// <param name="value">The resolved value. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        protected VirtualProperty(string entity, string property, TProperty value) : base(entity, property, value) 
        {
            value.ThrowIfNull();
        }

        /// <summary>
        /// Returns the resolved non-null value of the virtual property.
        /// </summary>
        /// <returns>The resolved <typeparamref name="TProperty"/> value.</returns>
        /// <exception cref="VirtualPropertyAccessException">Thrown if the property is not resolved.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the resolved value is null (should not occur due to constructor enforcement).</exception>
        public TProperty GetValueOrThrow() => InternalGetRequiredValueOrThrow();
    }

    /// <summary>
    /// Represents a sealed, non-nullable virtual property that does not require subclassing.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a built-in sealed variant of <see cref="VirtualProperty{TEntity, TProperty, TSelf}"/> 
    /// that simplifies usage for cases where defining a dedicated subclass would be excessive.
    /// </para>
    ///
    /// <para>
    /// Internally, this type relies on reflection to construct instances, even though its constructors are <c>private</c>.
    /// This is enabled via reflection and constructor metadata caching inside the virtualization infrastructure.
    /// </para>
    ///
    /// <para>
    /// This type must be instantiated via <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Unresolved"/> and 
    /// <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Resolve"/> factory methods only.
    /// Public constructors are intentionally not exposed.
    /// </para>
    /// </remarks>
    /// <typeparam name="TEntity">The entity type that owns the property.</typeparam>
    /// <typeparam name="TProperty">The non-nullable value type of the virtual property.</typeparam>
    public sealed record VirtualProperty<TEntity, TProperty> : VirtualProperty<TEntity, TProperty, VirtualProperty<TEntity, TProperty>>
        where TEntity : IDomainEntity
        where TProperty : notnull
    {
        /// <summary>
        /// Private constructor for use by reflection during unresolved factory creation.
        /// </summary>
        /// <param name="expression">A lambda pointing to the unresolved property.</param>
        private VirtualProperty(Expression<Func<TEntity, TProperty>> expression)
            : base(expression)
        {
        }

        /// <summary>
        /// Private constructor for use by reflection during resolution with a known value.
        /// </summary>
        /// <param name="entity">The owning entity name.</param>
        /// <param name="property">The property name.</param>
        /// <param name="value">The non-null resolved value.</param>
        private VirtualProperty(string entity, string property, TProperty value)
            : base(entity, property, value)
        {
        }
    }
}
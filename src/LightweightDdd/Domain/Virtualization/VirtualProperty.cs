// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using LightweightDdd.Domain.Entity;
using LightweightDdd.Domain.Virtualization.Exceptions;
using LightweightDdd.Extensions;

namespace LightweightDdd.Domain.Virtualization
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
        /// Initializes a new unresolved instance of the virtual property using explicit entity and property names.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the property being virtualized.</param>
        protected VirtualProperty(string entityName, string propertyName)
            : base(entityName, propertyName) 
        { 
        }

        /// <summary>
        /// Initializes a resolved instance of the virtual property with a required (non-null) value.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the property being virtualized.</param>
        /// <param name="value">The resolved value to assign. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="value"/> is <c>null</c>, as this virtual property does not allow null values.
        /// </exception>
        protected VirtualProperty(string entityName, string propertyName, TProperty value)
            : base(entityName, propertyName, value)
        {
            value.ThrowIfNull();
        }

        /// <summary>
        /// Returns the resolved non-null value of the virtual property.
        /// </summary>
        /// <returns>The resolved <typeparamref name="TProperty"/> value.</returns>
        /// <exception cref="VirtualPropertyAccessException">Thrown if the property is not resolved.</exception>
        public TProperty GetValueOrThrow() => InternalGetValueOrThrow()!;

        /// <summary>
        /// Validates that the resolved value is not <c>null</c>.
        /// This implementation enforces the non-nullability contract of <see cref="VirtualProperty{TEntity, TProperty, TSelf}"/>.
        /// If <c>null</c> is passed, a <see cref="VirtualPropertyValueException"/> is thrown with contextual information.
        /// </summary>
        /// <param name="value">The value to validate during resolution.</param>
        /// <exception cref="VirtualPropertyValueException">
        /// Thrown when <paramref name="value"/> is <c>null</c>, as this virtual property does not allow null values.
        /// </exception>
        protected override void ValidateResolvedValue(TProperty? value)
        {
            if (value is not null)
            {
                return;
            }

            throw new VirtualPropertyValueException(
                entityName: EntityName,
                propertyName: PropertyName,
                message: $"Null value is not allowed for virtual property '{PropertyName}' on entity '{EntityName}'."
            );
        }
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
        /// Initializes a new unresolved instance of the virtual property using explicit entity and property names.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the property being virtualized.</param>
        private VirtualProperty(string entityName, string propertyName)
            : base(entityName, propertyName)
        {
        }

        /// <summary>
        /// Initializes a resolved instance of the virtual property with a required (non-null) value.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the property being virtualized.</param>
        /// <param name="value">The resolved value to assign. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="value"/> is <c>null</c>, as this virtual property does not allow null values.
        /// </exception>
        private VirtualProperty(string entityName, string propertyName, TProperty value)
            : base(entityName, propertyName, value)
        {
        }
    }
}
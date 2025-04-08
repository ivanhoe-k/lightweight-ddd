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
        /// <param name="hasChanged">
        /// Indicates whether the value was set via domain logic (<c>true</c>) or via hydration (<c>false</c>).
        /// </param>
        /// <param name="value">The resolved value to assign. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="value"/> is <c>null</c>, as this virtual property does not allow null values.
        /// </exception>
        protected VirtualProperty(string entityName, string propertyName, bool hasChanged, TProperty value)
            : base(entityName, propertyName, hasChanged, value)
        {
            value.ThrowIfNull();
        }

        /// <summary>
        /// Validates that the resolved value is not <c>null</c>.
        /// This implementation enforces the non-nullability contract of <see cref="VirtualProperty{TEntity, TProperty, TSelf}"/>.
        /// If <c>null</c> is passed, a <see cref="VirtualPropertyValueException"/> is thrown with contextual information.
        /// </summary>
        /// <param name="value">The value to validate during resolution.</param>
        /// <exception cref="VirtualPropertyValueException">
        /// Thrown when <paramref name="value"/> is <c>null</c>, as this virtual property does not allow null values.
        /// </exception>
        protected override void ValidateResolvedValue(TProperty value)
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
    /// designed for convenience when defining a custom subclass is unnecessary.
    /// </para>
    ///
    /// <para>
    /// This type participates fully in the Virtual Entity Pattern, supporting both <b>hydration</b>
    /// (via <see cref="IResolvable{TEntity, TProperty, TVirtual}.Resolve"/>) and
    /// domain-level <b>mutation</b> (via <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Update"/>).
    /// </para>
    ///
    /// <para>
    /// All instantiation is done via internal infrastructure using reflection, based on metadata cached
    /// by <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}"/>. Although constructors are <c>private</c>,
    /// the virtualization layer handles creation via cached <see cref="System.Reflection.ConstructorInfo"/> instances.
    /// </para>
    ///
    /// <para>
    /// Consumers should use static factory methods such as <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Unresolved"/>
    /// or rely on infrastructure like <see cref="VirtualArgsBuilderBase{TEntity, TArgs}"/> to hydrate this property.
    /// Public construction is intentionally unsupported and will result in runtime failure.
    /// </para>
    /// </remarks>
    /// <typeparam name="TEntity">The domain entity type that owns this virtual property.</typeparam>
    /// <typeparam name="TProperty">The non-nullable value type represented by this virtual property.</typeparam>
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
        /// <param name="hasChanged">
        /// Indicates whether the value was set via domain logic (<c>true</c>) or via hydration (<c>false</c>).
        /// </param>
        /// <param name="value">The resolved value to assign. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="value"/> is <c>null</c>, as this virtual property does not allow null values.
        /// </exception>
        private VirtualProperty(string entityName, string propertyName, bool hasChanged, TProperty value)
            : base(entityName, propertyName, hasChanged, value)
        {
        }
    }
}
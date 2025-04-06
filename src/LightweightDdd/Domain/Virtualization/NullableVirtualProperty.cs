// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Entity;
using LightweightDdd.Domain.Virtualization.Exceptions;
using LightweightDdd.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Domain.Virtualization
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
        /// Initializes a new unresolved instance of the virtual property using explicit entity and property names.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the property being virtualized.</param>
        protected NullableVirtualProperty(string entityName, string propertyName) 
            : base(entityName, propertyName) 
        { 
        }

        /// <summary>
        /// Initializes a resolved instance of the virtual property with a nullable value.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the property being virtualized.</param>
        /// <param name="value">The resolved value to assign. May be <c>null</c>.</param>
        protected NullableVirtualProperty(string entityName, string propertyName, TProperty? value) 
            : base(entityName, propertyName, value)
        { 
        }

        /// <summary>
        /// Overrides the value validation hook to allow <c>null</c> values during resolution.
        /// This implementation represents a virtual property that explicitly permits <c>null</c>,
        /// in contrast to <see cref="VirtualProperty{TEntity, TProperty, TSelf}"/>, which enforces non-nullability.
        /// </summary>
        /// <param name="value">The value to validate during resolution. <c>null</c> is allowed.</param>
        protected override void ValidateResolvedValue(TProperty? value)
        {
            return;
        }
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
        /// Initializes a new unresolved instance of the virtual property using explicit entity and property names.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the property being virtualized.</param>
        private NullableVirtualProperty(string entityName, string propertyName) 
            : base(entityName, propertyName) 
        {
        }

        /// <summary>
        /// Initializes a resolved instance of the virtual property with a nullable value.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the property being virtualized.</param>
        /// <param name="value">The resolved value to assign. May be <c>null</c>.</param>
        private NullableVirtualProperty(string entityName, string propertyName, TProperty? value) 
            : base(entityName, propertyName, value) 
        {
        }
    }
}

// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using LightweightDdd.DomainModel;
using LightweightDdd.Extensions;
using LightweightDdd.Utilities;

namespace LightweightDdd.Virtualization
{
    /// <summary>
    /// Represents an immutable virtualized property within a partially materialized domain entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the domain entity the property belongs to.</typeparam>
    /// <typeparam name="TProperty">The type of the property value.</typeparam>
    /// <typeparam name="TSelf">
    /// The concrete type implementing this virtual property (used for fluent resolution).
    /// Uses the Curiously Recurring Template Pattern (CRTP) for fluent, type-safe construction.
    /// </typeparam>
    /// <remarks>
    /// This abstraction supports the Virtual Entity Pattern and is designed to be immutable.
    /// It enables partial loading of aggregates while guarding against access to unresolved or unavailable data.
    ///
    /// A virtual property must be explicitly resolved via <see cref="Resolve"/> before being accessed.
    /// Attempting to access it prematurely will throw a <see cref="VirtualPropertyAccessException"/>.
    /// Use <see cref="GetValueOrThrow"/> to retrieve nullable values, or <see cref="GetRequiredValueOrThrow"/> to enforce non-null semantics.
    /// </remarks>
    public record VirtualProperty<TEntity, TProperty, TSelf>
        where TEntity : IDomainEntity
        where TSelf : VirtualProperty<TEntity, TProperty, TSelf>
    {
        private readonly TProperty? _value;
        private readonly bool _isResolved;
        private readonly string _property;
        private readonly string _entity;

        /// <summary>
        /// Initializes an unresolved virtual property.
        /// </summary>
        /// <param name="propertyExp">A lambda expression identifying the property on the entity.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyExp"/> is null.</exception>
        protected VirtualProperty(Expression<Func<TEntity, TProperty>> propertyExp)
        {
            propertyExp.ThrowIfNull();

            _value = default;
            _isResolved = false;
            _property = ExpressionUtils.GetPropertyPath(propertyExp);
            _entity = typeof(TEntity).Name;
        }

        /// <summary>
        /// Initializes a resolved virtual property with a value.
        /// </summary>
        /// <param name="entity">The name of the owning entity.</param>
        /// <param name="property">The name of the virtualized property.</param>
        /// <param name="value">The resolved value of the property.</param>
        protected VirtualProperty(string entity, string property, TProperty? value)
        {
            _entity = entity;
            _property = property;
            _isResolved = true;
            _value = value;
        }

        /// <summary>
        /// Resolves the virtual property with the given value.
        /// </summary>
        /// <param name="value">The value to assign to the property.</param>
        /// <returns>A new, resolved instance of <typeparamref name="TSelf"/>.</returns>
        public TSelf Resolve(TProperty? value)
        {
            return (TSelf)Activator.CreateInstance(typeof(TSelf), _entity, _property, value)!;
        }

        /// <summary>
        /// Returns the property value if it has been resolved; otherwise, throws an exception.
        /// </summary>
        /// <returns>The resolved value of the property, which may be null.</returns>
        /// <exception cref="VirtualPropertyAccessException">Thrown if the property has not been resolved.</exception>
        public TProperty? GetValueOrThrow()
        {
            if (!_isResolved)
            {
                throw new VirtualPropertyAccessException(_property, _entity);
            }

            return _value;
        }

        /// <summary>
        /// Returns the property value if it has been resolved and is not null; otherwise, throws an exception.
        /// </summary>
        /// <returns>The resolved, non-null value of the property.</returns>
        /// <exception cref="VirtualPropertyAccessException">Thrown if the property has not been resolved.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the resolved value is null.</exception>
        public TProperty GetRequiredValueOrThrow()
        {
            if (!_isResolved)
            {
                throw new VirtualPropertyAccessException(_property, _entity);
            }

            if (_value is null)
            {
                throw new InvalidOperationException($"Property '{_property}' on entity '{_entity}' was resolved but contains a null value.");
            }

            return _value;
        }
    }
}

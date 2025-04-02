// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Core.DomainModel;
using LightweightDdd.Core.Utilities;
using System.Linq.Expressions;
using System;
using LightweightDdd.Core.Extensions;
using System.Reflection;

namespace LightweightDdd.Core.Virtualization
{
    /// <summary>
    /// Represents the foundational base type for all virtual properties in the Virtual Entity Pattern.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Virtual properties are used to represent values that may be either explicitly resolved or intentionally left unresolved
    /// during projection, transformation, or selective loading scenarios.
    /// </para>
    ///
    /// <para>
    /// This type uses the <i>Curiously Recurring Template Pattern (CRTP)</i> via the <typeparamref name="TSelf"/> parameter
    /// to ensure that static factory methods (<see cref="CreateFor"/>, <see cref="Resolve(TProperty?)"/>) return the correct concrete type.
    /// </para>
    ///
    /// <para>
    /// Consumers are encouraged to use one of the built-in sealed variants when modeling virtual properties:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <see cref="VirtualProperty{TEntity, TProperty}"/> – for non-nullable properties
    /// </item>
    /// <item>
    /// <see cref="NullableVirtualProperty{TEntity, TProperty}"/> – for nullable properties
    /// </item>
    /// </list>
    ///
    /// <para>
    /// These types are designed for consistent usage, encapsulation, and safety. Constructors in leaf types should always be
    /// declared as <b>private</b> or <b>protected</b> to prevent uncontrolled instantiation.
    /// Public constructors are <b>not supported</b> and will trigger a runtime failure due to enforced reflection-based instantiation rules.
    /// </para>
    ///
    /// <para>
    /// Advanced consumers may define custom virtual property types (e.g., <c>VirtualUserProperty{TProperty}</c>)
    /// by inheriting from either <see cref="VirtualProperty{TEntity, TProperty, TSelf}"/> or
    /// <see cref="NullableVirtualProperty{TEntity, TProperty, TSelf}"/> depending on the nullability of the value.
    /// This allows encapsulating virtual properties for a specific domain entity (e.g., <c>VirtualUser</c>)
    /// while preserving a consistent naming convention: <c>Virtual&lt;EntityName&gt;Property</c>.
    /// </para>
    /// </remarks>
    /// <typeparam name="TEntity">
    /// The aggregate root or domain entity type that owns this virtual property.
    /// </typeparam>
    /// <typeparam name="TProperty">
    /// The value type of the virtual property (can be a value object, primitive, or collection).
    /// </typeparam>
    /// <typeparam name="TSelf">
    /// The self-referencing subtype that inherits from this base class.
    /// Used for enforcing a clean CRTP-style fluent interface and proper static instantiation.
    /// </typeparam>
    public abstract record VirtualPropertyBase<TEntity, TProperty, TSelf>
        where TEntity : IDomainEntity
        where TSelf : VirtualPropertyBase<TEntity, TProperty, TSelf>
    {
        private static readonly object _initLock = new();
        private static ConstructorInfo? _expressionCtor;
        private static ConstructorInfo? _resolvedCtor;
        private static bool _initialized;

        private readonly TProperty? _value;
        private readonly bool _isResolved;
        private readonly string _property;
        private readonly string _entity;

        /// <summary>
        /// Initializes an unresolved virtual property using a strongly typed expression.
        /// </summary>
        /// <param name="propertyExp">A lambda expression identifying the property being virtualized.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyExp"/> is null.</exception>
        protected VirtualPropertyBase(Expression<Func<TEntity, TProperty>> propertyExp)
        {
            propertyExp.ThrowIfNull();

            _value = default;
            _isResolved = false;
            _property = ExpressionUtils.GetPropertyPath(propertyExp);
            _entity = typeof(TEntity).Name;
        }

        /// <summary>
        /// Initializes a resolved virtual property with a concrete value.
        /// </summary>
        /// <param name="entity">The name of the owning entity.</param>
        /// <param name="property">The name of the virtual property.</param>
        /// <param name="value">The resolved value.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="entity"/> or <paramref name="property"/> is null or whitespace.</exception>
        protected VirtualPropertyBase(string entity, string property, TProperty? value)
        {
            entity.ThrowIfNullOrWhiteSpace();
            property.ThrowIfNullOrWhiteSpace();

            _entity = entity;
            _property = property;
            _isResolved = true;
            _value = value;
        }

        /// <summary>
        /// Creates a new unresolved virtual property of type <typeparamref name="TSelf"/>.
        /// </summary>
        /// <param name="propertyExp">A strongly typed lambda expression pointing to the property being virtualized.</param>
        /// <returns>A new instance of <typeparamref name="TSelf"/> representing an unresolved virtual property.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a matching non-public constructor is not found. Only private/protected constructors are allowed.
        /// </exception>
        public static TSelf CreateFor(Expression<Func<TEntity, TProperty>> propertyExp)
        {
            propertyExp.ThrowIfNull();

            EnsureConstructorCacheInitialized();
            return (TSelf)_expressionCtor!.Invoke(new object[] { propertyExp });
        }

        /// <summary>
        /// Creates a new resolved copy of the current virtual property.
        /// </summary>
        /// <param name="value">The value to resolve this property with.</param>
        /// <returns>A new resolved instance of <typeparamref name="TSelf"/> with the given value.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a matching non-public constructor is not found. Only private/protected constructors are allowed.
        /// </exception>
        public TSelf Resolve(TProperty? value)
        {
            EnsureConstructorCacheInitialized();

            return (TSelf)_resolvedCtor!.Invoke(new object[] { _entity, _property, value });
        }

        /// <summary>
        /// Gets the current value of the property, even if it is null.
        /// </summary>
        /// <returns>The resolved value, or null.</returns>
        /// <exception cref="VirtualPropertyAccessException">Thrown if the property is not resolved.</exception>
        protected TProperty? InternalGetValueOrThrow()
        {
            if (!_isResolved)
            {
                throw new VirtualPropertyAccessException(_property, _entity);
            }

            return _value;
        }

        /// <summary>
        /// Gets the current value of the property, assuming it is non-null.
        /// </summary>
        /// <returns>The resolved, non-null value of the property.</returns>
        /// <exception cref="VirtualPropertyAccessException">Thrown if the property is not resolved.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the resolved value is null.</exception>
        protected TProperty InternalGetRequiredValueOrThrow()
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

        /// <summary>
        /// Initializes and caches constructor information via reflection.
        /// </summary>
        /// <remarks>
        /// This method is called once per closed generic type (TSelf). It caches the constructors required
        /// to support runtime creation via <see cref="CreateFor"/> and <see cref="Resolve"/>.
        /// Only non-public constructors are allowed. Public constructors will cause a failure.
        /// </remarks>
        private static void EnsureConstructorCacheInitialized()
        {
            if (_initialized)
            {
                return;
            }

            lock (_initLock)
            {
                if (_initialized)
                {
                    return;
                }

                _expressionCtor = ReflectionUtils.GetVirtualPropertyConstructorOrThrow(
                   entityType: typeof(TEntity),
                   propertyType: typeof(TProperty),
                   virtualPropertyType: typeof(TSelf),
                   isExpressionCtor: true,
                   bindingFlags: BindingFlags.Instance | BindingFlags.NonPublic);

                _resolvedCtor = ReflectionUtils.GetVirtualPropertyConstructorOrThrow(
                   entityType: typeof(TEntity),
                   propertyType: typeof(TProperty),
                   virtualPropertyType: typeof(TSelf),
                   isExpressionCtor: false,
                   bindingFlags: BindingFlags.Instance | BindingFlags.NonPublic);

                _initialized = true;
            }
        }
    }
}

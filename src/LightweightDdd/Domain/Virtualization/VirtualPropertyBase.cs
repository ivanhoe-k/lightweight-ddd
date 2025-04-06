// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Linq.Expressions;
using System;
using LightweightDdd.Extensions;
using System.Reflection;
using LightweightDdd.Domain.Entity;
using LightweightDdd.Domain.Virtualization.Exceptions;

namespace LightweightDdd.Domain.Virtualization
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
    /// to ensure that static factory methods (<see cref="Unresolved"/>, <see cref="Resolve(TProperty?)"/>) return the correct concrete type.
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

        /// <summary>
        /// Cached constructor used when creating unresolved virtual property instances
        /// with explicit entity and property names (from parsed expressions).
        /// </summary>
        private static ConstructorInfo? _unresolvedCtor;

        /// <summary>
        /// Cached constructor used when creating resolved virtual property instances
        /// with entity, property, and value provided.
        /// </summary>
        private static ConstructorInfo? _resolvedCtor;

        private static bool _initialized;

        private readonly TProperty? _value;
        private readonly bool _isResolved;
        private readonly string _propertyName;
        private readonly string _entityName;

        /// <summary>
        /// Initializes an unresolved virtual property using explicit entity and property names.
        /// Intended for internal or framework-level use where property expression parsing is not needed or available.
        /// </summary>
        /// <param name="entityName">The name of the entity that owns the virtual property.</param>
        /// <param name="propertyName">The name of the property being virtualized.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="entityName"/> or <paramref name="propertyName"/> is <c>null</c> or whitespace.
        /// </exception>
        protected VirtualPropertyBase(string entityName, string propertyName)
        {
            entityName.ThrowIfNullOrWhiteSpace();
            propertyName.ThrowIfNullOrWhiteSpace();

            _value = default;
            _isResolved = false;
            _propertyName = propertyName;
            _entityName = entityName;
        }

        /// <summary>
        /// Initializes a resolved virtual property with a concrete value.
        /// </summary>
        /// <param name="entityName">The name of the owning entity.</param>
        /// <param name="propertyName">The name of the virtual property.</param>
        /// <param name="value">The resolved value.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="entityName"/> or <paramref name="propertyName"/> is null or whitespace.</exception>
        protected VirtualPropertyBase(string entityName, string propertyName, TProperty? value)
        {
            entityName.ThrowIfNullOrWhiteSpace();
            propertyName.ThrowIfNullOrWhiteSpace();

            _entityName = entityName;
            _propertyName = propertyName;
            _isResolved = true;
            _value = value;
        }

        /// <summary>
        /// Gets the name of the property being virtualized (i.e., the property represented by this virtual instance).
        /// </summary>
        public string PropertyName => _propertyName;

        /// <summary>
        /// Gets the name of the entity type that owns the virtualized property.
        /// </summary>
        public string EntityName => _entityName;

        /// <summary>
        /// Creates a new unresolved virtual property of type <typeparamref name="TSelf"/>.
        /// </summary>
        /// <param name="propertyExp">A strongly typed lambda expression pointing to the property being virtualized.</param>
        /// <returns>A new instance of <typeparamref name="TSelf"/> representing an unresolved virtual property.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a matching non-public constructor is not found. Only private/protected constructors are allowed.
        /// </exception>
        public static TSelf Unresolved(Expression<Func<TEntity, TProperty>> propertyExp)
        {
            propertyExp.ThrowIfNull();

            EnsureConstructorCacheInitialized();

            var propertyName = ExpressionHelper.GetPropertyPath(propertyExp);
            var entityName = typeof(TEntity).Name;

            return InvokeConstructor(_unresolvedCtor!, [entityName, propertyName]);
        }

        /// <summary>
        /// Creates a new resolved copy of the current virtual property with the specified value.
        /// </summary>
        /// <remarks>
        /// This method is idempotent and side-effect free. Calling <c>Resolve</c> multiple times
        /// is allowed and results in new immutable instances.
        /// </remarks>
        /// <param name="value">The value to resolve this property with.</param>
        /// <returns>A new resolved instance of <typeparamref name="TSelf"/>.</returns>
        public TSelf Resolve(TProperty value)
        {
            ValidateResolvedValue(value);

            return InvokeConstructor(_resolvedCtor!, [_entityName, _propertyName, value]);
        }

        /// <summary>
        /// Hook method for subclasses to validate or enforce constraints on the resolved value.
        /// Must be overridden in subclasses to implement validation.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected abstract void ValidateResolvedValue(TProperty value);

        /// <summary>
        /// Returns the resolved value of the virtual property.
        /// </summary>
        /// <returns>The resolved value.</returns>
        /// <exception cref="VirtualPropertyAccessException">
        /// Thrown if the property has not been resolved yet.
        /// </exception>
        public TProperty GetValueOrThrow()
        {
            if (!_isResolved)
            {
                throw new VirtualPropertyAccessException(_entityName, _propertyName);
            }

            return _value!;
        }

        /// <summary>
        /// Initializes and caches constructor information via reflection.
        /// </summary>
        /// <remarks>
        /// This method is called once per closed generic type (<typeparamref name="TSelf"/>).
        /// It resolves and caches the two required constructors needed to support runtime creation via
        /// <see cref="Unresolved"/> and <see cref="Resolve"/>:
        /// one for expression-based initialization and one for direct value-based resolution.
        /// <para>
        /// Only non-public constructors (private or protected) are allowed.
        /// Public constructors will not be considered and their presence will not prevent failure if private ones are missing.
        /// </para>
        /// </remarks>
        /// <exception cref="VirtualPropertyConstructorResolutionException">
        /// Thrown if the required constructor(s) cannot be found.
        /// </exception>
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

                _unresolvedCtor = GetOrThrowConstructor(
                    propertyType: typeof(TProperty),
                    virtualPropertyType: typeof(TSelf),
                    isUnresolvedCtor: true);

                _resolvedCtor = GetOrThrowConstructor(
                    propertyType: typeof(TProperty),
                    virtualPropertyType: typeof(TSelf),
                    isUnresolvedCtor: false);

                _initialized = true;
            }
        }

        private static ConstructorInfo GetOrThrowConstructor(
            Type propertyType,
            Type virtualPropertyType,
            bool isUnresolvedCtor)
        {
            var ctor = ReflectionHelper.GetVirtualPropertyConstructor(
                propertyType: propertyType,
                virtualPropertyType: virtualPropertyType,
                isUnresolvedCtor: isUnresolvedCtor,
                bindingFlags: BindingFlags.Instance | BindingFlags.NonPublic);

            if (ctor is null)
            {
                throw new VirtualPropertyConstructorResolutionException(
                    virtualPropertyType: virtualPropertyType, 
                    attemptedParams: ReflectionHelper.GetVirtualPropertyConstructorTypes(propertyType, isUnresolvedCtor));
            }

            return ctor;
        }

        /// <summary>
        /// Invokes the specified non-public constructor with the provided arguments and casts the result to <typeparamref name="TSelf"/>.
        /// </summary>
        /// <param name="ctor">The constructor to invoke.</param>
        /// <param name="args">The arguments to pass to the constructor.</param>
        /// <returns>A new instance of <typeparamref name="TSelf"/>.</returns>
        /// <exception cref="Exception">
        /// Re-throws any exception thrown by the constructor body directly, unwrapped from <see cref="TargetInvocationException"/>.
        /// </exception>
        /// <remarks>
        /// This method ensures that any exception thrown by the invoked constructor is not wrapped in a
        /// <see cref="TargetInvocationException"/>, making error handling and unit testing more predictable.
        /// </remarks>
        private static TSelf InvokeConstructor(ConstructorInfo ctor, object?[] args)
        {
            try
            {
                return (TSelf)ctor.Invoke(args);
            }
            catch (TargetInvocationException ex) when (ex.InnerException is VirtualPropertyException inner)
            {
                // Re-throw the actual exception for clearer diagnostics
                throw inner;
            }
        }
    }
}

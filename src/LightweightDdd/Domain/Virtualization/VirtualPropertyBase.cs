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
    /// Virtual properties are designed to represent fields within a virtualized domain entity that may be
    /// <b>partially hydrated</b> based on the needs of a specific use case. A property can either remain
    /// unresolved (e.g., intentionally skipped during projection) or be resolved through controlled mechanisms
    /// during initialization or mutation.
    /// </para>
    ///
    /// <para>
    /// This base type supports two distinct flows:
    /// </para>
    /// <list type="bullet">
    ///   <item>
    ///     <b>Hydration</b>: The property is resolved once using the internal <see cref="IResolvable{TEntity, TProperty, TSelf}.Resolve"/>
    ///     method, typically via a builder such as <see cref="VirtualArgsBuilderBase{TEntity, TArgs}"/>. This sets the <see cref="HasResolved"/>
    ///     flag without marking the property as changed.
    ///   </item>
    ///   <item>
    ///     <b>Mutation</b>: The property is explicitly updated using <see cref="VirtualPropertyBase{TEntity, TProperty, TSelf}.Update"/>, which sets both <see cref="HasResolved"/>
    ///     and <see cref="HasChanged"/> to <c>true</c>. This is intended for domain-level modifications.
    ///   </item>
    /// </list>
    ///
    /// <para>
    /// This type uses the <i>Curiously Recurring Template Pattern (CRTP)</i> via the <typeparamref name="TSelf"/> parameter
    /// to ensure that strongly typed factory methods such as <see cref="Unresolved"/> and resolution flows return the correct
    /// virtual property subtype.
    /// </para>
    ///
    /// <para>
    /// Consumers are encouraged to use one of the built-in sealed variants:
    /// </para>
    /// <list type="bullet">
    ///   <item>
    ///     <see cref="VirtualProperty{TEntity, TProperty}"/> – for non-nullable virtual properties
    ///   </item>
    ///   <item>
    ///     <see cref="NullableVirtualProperty{TEntity, TProperty}"/> – for nullable virtual properties
    ///   </item>
    /// </list>
    ///
    /// <para>
    /// To ensure consistency and controlled instantiation, concrete virtual property types must declare their constructors as
    /// <b>private</b> or <b>protected</b>. Public constructors are not supported and will cause a runtime failure during hydration.
    /// </para>
    ///
    /// <para>
    /// Advanced consumers may define custom virtual property types (e.g., <c>VirtualUserNameProperty</c>) by inheriting from
    /// either <see cref="VirtualProperty{TEntity, TProperty, TSelf}"/> or
    /// <see cref="NullableVirtualProperty{TEntity, TProperty, TSelf}"/> depending on the nullability of the value type.
    /// This enables consistent modeling per entity and improves discoverability of partial projections.
    /// </para>
    /// </remarks>
    /// <typeparam name="TEntity">
    /// The domain entity (typically an aggregate root) that owns this virtual property.
    /// </typeparam>
    /// <typeparam name="TProperty">
    /// The value type held by this virtual property (value object, primitive, collection, etc.).
    /// </typeparam>
    /// <typeparam name="TSelf">
    /// The self-referencing type implementing this base class. Used for CRTP-based static typing and fluent APIs.
    /// </typeparam>
    public abstract record VirtualPropertyBase<TEntity, TProperty, TSelf> 
            : IVirtualProperty, IResolvable<TEntity, TProperty, TSelf>
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
        private static ConstructorInfo? _resolvedWithChangeTrackingCtor;

        private static bool _initialized;

        private readonly TProperty? _value;
        private readonly bool _hasResolved;
        private readonly bool _hasChanged;
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
            _hasResolved = false;
            _hasChanged = false;
            _propertyName = propertyName;
            _entityName = entityName;
        }

        /// <summary>
        /// Initializes a resolved virtual property with a concrete value and change tracking metadata.
        /// </summary>
        /// <param name="entityName">The name of the domain entity associated with the property.</param>
        /// <param name="propertyName">The name of the virtual property being initialized.</param>
        /// <param name="hasChanged">
        /// A value indicating whether the property was changed via domain logic (<c>true</c>)
        /// or resolved through hydration (<c>false</c>).
        /// </param>
        /// <param name="value">The resolved or updated value of the property.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="entityName"/> or <paramref name="propertyName"/> is <c>null</c> or whitespace.
        /// </exception>
        protected VirtualPropertyBase(string entityName, string propertyName, bool hasChanged, TProperty? value)
        {
            entityName.ThrowIfNullOrWhiteSpace();
            propertyName.ThrowIfNullOrWhiteSpace();

            _entityName = entityName;
            _propertyName = propertyName;
            _hasResolved = true;
            _hasChanged = hasChanged;
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
        /// Gets a value indicating whether this virtual property has been resolved through hydration or mutation.
        /// </summary>
        /// <remarks>
        /// This flag is set when the property is initialized using either <see cref="IResolvable{TEntity, TProperty, TVirtual}.Resolve"/>
        /// during the hydration phase, or via the <c>Update(...)</c> method as part of a domain mutation.
        ///
        /// It guarantees that <c>GetValueOrThrow()</c> will no longer throw and that the value is available for use.
        /// </remarks>
        public bool HasResolved => _hasResolved;

        /// <summary>
        /// Gets a value indicating whether the virtual property was explicitly changed via a mutation.
        /// </summary>
        /// <remarks>
        /// This flag is set when <c>Update(...)</c> is called — even if the property was never previously hydrated via
        /// <see cref="IResolvable{TEntity, TProperty, TVirtual}.Resolve"/>.
        ///
        /// This enables change tracking for domain scenarios where the previous value is not needed,
        /// such as blind overwrites or commands where only the new value matters.
        /// </remarks>
        public bool HasChanged => _hasChanged;

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
        /// Hydrates the virtual property with the specified value and returns a new resolved instance.
        /// </summary>
        /// <remarks>
        /// This method is intended strictly for infrastructure use during the <b>hydration phase</b>
        /// of a virtual entity — typically by <see cref="VirtualArgsBuilderBase{TEntity, TArgs}"/>.
        /// It should <b>not</b> be used for domain-level updates or business mutations.
        ///
        /// For applying domain-driven changes, use <see cref="Update"/> instead.
        ///
        /// <para>
        /// This method enforces single-use semantics. Calling it on an already resolved property will throw
        /// a <see cref="VirtualPropertyResolutionException"/>.
        /// </para>
        /// </remarks>
        /// <param name="value">The value to hydrate the virtual property with.</param>
        /// <returns>
        /// A new instance of the virtual property marked as resolved, but not changed.
        /// </returns>
        /// <exception cref="VirtualPropertyResolutionException">
        /// Thrown if the property has already been hydrated. Indicates an improper second resolution,
        /// typically due to builder misuse or logic error.
        /// </exception>
        TSelf IResolvable<TEntity, TProperty, TSelf>.Resolve(TProperty value)
        {
            if (_hasResolved)
            {
                throw new VirtualPropertyResolutionException(_entityName, _propertyName);
            }

            ValidateResolvedValue(value);

            return InvokeConstructor(_resolvedWithChangeTrackingCtor!, [_entityName, _propertyName, _hasChanged, value]);
        }

        /// <summary>
        /// Applies a mutation to the virtual property using the specified value.
        /// </summary>
        /// <remarks>
        /// This method should be used when the virtual property is modified as part of a business use case,
        /// independent of whether the original value was hydrated.
        ///
        /// It sets both <see cref="HasResolved"/> and <see cref="HasChanged"/> flags, enabling
        /// downstream logic (such as patch persistence) to track intentional updates.
        ///
        /// Unlike <see cref="IResolvable{TEntity, TProperty, TSelf}.Resolve"/>, this method can be called multiple times safely.
        /// </remarks>
        /// <param name="value">The new value to apply to the virtual property.</param>
        /// <returns>
        /// A new instance of the virtual property marked as both resolved and changed.
        /// </returns>
        public TSelf Update(TProperty value)
        {
            ValidateResolvedValue(value);

            var hasChanged = true;

            return InvokeConstructor(_resolvedWithChangeTrackingCtor!, [_entityName, _propertyName, hasChanged, value]);
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
            if (!_hasResolved)
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

                _resolvedWithChangeTrackingCtor = GetOrThrowConstructor(
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
        private static TSelf InvokeConstructor(ConstructorInfo ctor, object?[] args)
        {
            return (TSelf)ctor.Invoke(args);
        }
    }
}

// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using FluentAssertions;
using System.Linq.Expressions;
using LightweightDdd.Domain.Virtualization;
using LightweightDdd.Domain.Virtualization.Exceptions;
using LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization
{
    public sealed class VirtualPropertyTests
    {
        [Fact]
        public void VirtualProperty_ShouldTrackChange_WhenUpdated()
        {
            // Arrange
            var property = VirtualProperty<DummyEntity, int>.Unresolved(x => x.Age);

            // Act
            var updated = property.Update(123);

            // Assert
            updated.GetValueOrThrow().Should().Be(123);
            updated.HasChanged.Should().BeTrue();
        }

        [Fact]
        public void VirtualProperty_ShouldAllowMultipleUpdates()
        {
            // Arrange
            var property = VirtualProperty<DummyEntity, int>.Unresolved(x => x.Age);

            // Act
            var updated1 = property.Update(1);
            var updated2 = updated1.Update(2);

            // Assert
            updated1.GetValueOrThrow().Should().Be(1);
            updated2.GetValueOrThrow().Should().Be(2);
        }

        [Fact]
        public void VirtualProperty_ShouldBeResolved_WhenUpdated()
        {
            // Arrange
            var property = VirtualProperty<DummyEntity, int>.Unresolved(x => x.Age);

            // Act
            var updated = property.Update(55);

            // Assert
            updated.HasResolved.Should().BeTrue();
        }

        [Fact]
        public void NullableVirtualProperty_ShouldAllowUpdate_WithNull()
        {
            // Arrange
            var property = NullableVirtualProperty<DummyEntity, string?>.Unresolved(x => x.OptionalName);

            // Act
            var updated = property.Update(null);

            // Assert
            updated.GetValueOrThrow().Should().BeNull();
            updated.HasChanged.Should().BeTrue();
        }

        [Fact]
        public void VirtualProperty_ShouldThrow_WhenUpdatedWithNull()
        {
            // Arrange
            var property = VirtualProperty<DummyEntity, string>.Unresolved(x => x.Name);

            // Act
            Action act = () => property.Update(null!);

            // Assert
            act.Should().Throw<VirtualPropertyValueException>()
               .WithMessage($"Null value is not allowed for virtual property '{property.PropertyName}' on entity '{property.EntityName}'.");
        }

        [Fact]
        public void VirtualProperty_ShouldThrow_WhenAccessedUnresolved()
        {
            // Arrange
            var property = VirtualProperty<DummyEntity, int>.Unresolved(x => x.Age);

            // Act
            Action act = () => property.GetValueOrThrow();

            // Assert
            act.Should().Throw<VirtualPropertyAccessException>()
                .WithMessage("*Age*DummyEntity*");
        }

        [Fact]
        public void VirtualProperty_ShouldExposeCorrectEntityAndPropertyName()
        {
            // Arrange - Act
            var virtualProperty = VirtualProperty<DummyEntity, string>.Unresolved(x => x.Name);

            // Assert
            virtualProperty.EntityName.Should().Be(nameof(DummyEntity));
            virtualProperty.PropertyName.Should().Be(nameof(DummyEntity.Name));
        }

        [Fact]
        public void NullableVirtualProperty_ShouldThrow_WhenAccessedUnresolved()
        {
            // Arrange
            var property = NullableVirtualProperty<DummyEntity, string>.Unresolved(x => x.OptionalName);

            // Act
            Action act = () => property.GetValueOrThrow();

            // Assert
            act.Should().Throw<VirtualPropertyAccessException>()
                .WithMessage("*OptionalName*DummyEntity*");
        }

        [Fact]
        public void VirtualProperty_ShouldThrow_WhenUnresolvedReceivesNullExpression()
        {
            // Arrange
            Expression<Func<DummyEntity, int>>? expression = null;

            // Act
            Action act = () => VirtualProperty<DummyEntity, int>.Unresolved(expression!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("propertyExp");
        }

        [Fact]
        public void ResolveProperty_ShouldMarkAsResolved_WithoutChangeTracking()
        {
            // Arrange
            var builder = DummyEntityArgs.GetBuilder();

            // Act
            var args = builder.WithAge(42).Build();

            // Assert
            args.Age.GetValueOrThrow().Should().Be(42);
            args.Age.HasResolved.Should().BeTrue();
            args.Age.HasChanged.Should().BeFalse(); // Only resolved via builder
        }

        [Fact]
        public void ResolvedProperty_ShouldBecomeChanged_WhenUpdatedAfterHydration()
        {
            // Arrange
            var builder = DummyEntityArgs.GetBuilder();
            var args = builder.WithAge(42).Build();

            // Act
            var updated = args.Age.Update(99);

            // Assert
            updated.GetValueOrThrow().Should().Be(99);
            updated.HasResolved.Should().BeTrue();
            updated.HasChanged.Should().BeTrue(); // Because it was mutated after hydration
        }

    }
}

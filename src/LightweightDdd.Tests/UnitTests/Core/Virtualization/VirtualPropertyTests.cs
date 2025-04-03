// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using FluentAssertions;
using LightweightDdd.Core.DomainModel;
using LightweightDdd.Core.Virtualization;
using LightweightDdd.Core.Virtualization.Exceptions;
using LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers;
using System;
using System.Linq.Expressions;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization
{
    public sealed class VirtualPropertyTests
    {
        [Fact]
        public void VirtualProperty_ShouldThrow_WhenAccessedUnresolved()
        {
            // Arrange
            var property = VirtualProperty<DummyEntity, int>.CreateFor(x => x.Age);

            // Act
            Action act = () => property.GetValueOrThrow();

            // Assert
            act.Should().Throw<VirtualPropertyAccessException>()
                .WithMessage("*Age*DummyEntity*");
        }

        [Fact]
        public void VirtualProperty_ShouldReturnValue_WhenResolved()
        {
            // Arrange
            var expected = 100;
            var property = VirtualProperty<DummyEntity, int>.CreateFor(x => x.Age);

            // Act
            var resolved = property.Resolve(expected);

            // Assert
            resolved.GetValueOrThrow().Should().Be(expected);
        }

        [Fact]
        public void NullableVirtualProperty_ShouldThrow_WhenAccessedUnresolved()
        {
            // Arrange
            var property = NullableVirtualProperty<DummyEntity, string>.CreateFor(x => x.OptionalName);

            // Act
            Action act = () => property.GetValueOrThrow();

            // Assert
            act.Should().Throw<VirtualPropertyAccessException>()
                .WithMessage("*OptionalName*DummyEntity*");
        }

        [Fact]
        public void NullableVirtualProperty_ShouldReturnNull_WhenResolvedWithNull()
        {
            // Arrange
            var property = NullableVirtualProperty<DummyEntity, string>.CreateFor(x => x.OptionalName);

            // Act
            var resolved = property.Resolve(null);

            // Assert
            resolved.GetValueOrThrow().Should().BeNull();
        }

        [Fact]
        public void NullableVirtualProperty_ShouldReturnValue_WhenResolved()
        {
            // Arrange
            var expected = "resolved";
            var property = NullableVirtualProperty<DummyEntity, string>.CreateFor(x => x.OptionalName);

            // Act
            var resolved = property.Resolve(expected);

            // Assert
            resolved.GetValueOrThrow().Should().Be(expected);
        }

        [Fact]
        public void VirtualProperty_ShouldThrow_WhenCreateForReceivesNullExpression()
        {
            // Arrange
            Expression<Func<DummyEntity, int>>? expression = null;

            // Act
            Action act = () => VirtualProperty<DummyEntity, int>.CreateFor(expression!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("propertyExp");
        }

        [Fact]
        public void VirtualProperty_ShouldReturnNewResolved_WhenResolveCalledTwice()
        {
            // Arrange
            var first = 42;
            var secondValue = 99;
            var original = VirtualProperty<DummyEntity, int>.CreateFor(x => x.Age)
                .Resolve(first);

            // Act
            var second = original.Resolve(secondValue);

            // Assert
            second.GetValueOrThrow().Should().Be(secondValue);
            original.GetValueOrThrow().Should().Be(first);
        }

        [Fact]
        public void NullableVirtualProperty_ShouldSupportValueTypesWithNullability()
        {
            // Arrange
            var expected = 123;
            var property = NullableVirtualProperty<DummyEntity, int?>.CreateFor(x => x.Rating);

            // Act
            var resolvedWithNull = property.Resolve(null);
            var resolvedWithValue = property.Resolve(expected);

            // Assert
            resolvedWithNull.GetValueOrThrow().Should().BeNull();
            resolvedWithValue.GetValueOrThrow().Should().Be(expected);
        }
    }
}

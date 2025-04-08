// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using FluentAssertions;
using LightweightDdd.Extensions;
using LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers;
using System.Reflection;
using LightweightDdd.Domain.Virtualization.Exceptions;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization
{
    public sealed class VirtualCustomPropertyTests
    {
        [Fact]
        public void CustomVirtualProperty_ShouldTrackChange_WhenUpdated()
        {
            // Arrange
            var property = VirtualDummyProperty<int>.Unresolved(x => x.Age);

            // Act
            var updated = property.Update(42);

            // Assert
            updated.GetValueOrThrow().Should().Be(42);
            updated.HasChanged.Should().BeTrue();
        }

        [Fact]
        public void CustomVirtualProperty_ShouldAllowMultipleUpdates()
        {
            // Arrange
            var property = VirtualDummyProperty<int>.Unresolved(x => x.Age);

            // Act
            var updated1 = property.Update(100);
            var updated2 = updated1.Update(200);

            // Assert
            updated1.GetValueOrThrow().Should().Be(100);
            updated2.GetValueOrThrow().Should().Be(200);
            updated2.HasChanged.Should().BeTrue();
        }

        [Fact]
        public void CustomVirtualProperty_ShouldBeResolved_WhenUpdated()
        {
            // Arrange
            var property = VirtualDummyProperty<int>.Unresolved(x => x.Age);

            // Act
            var updated = property.Update(5);

            // Assert
            updated.HasResolved.Should().BeTrue();
        }

        [Fact]
        public void CustomNullableVirtualProperty_ShouldSupportUpdate_WithNull()
        {
            // Arrange
            var property = NullableVirtualDummyProperty<string?>.Unresolved(x => x.Name);

            // Act
            var updated = property.Update(null);

            // Assert
            updated.GetValueOrThrow().Should().BeNull();
            updated.HasChanged.Should().BeTrue();
        }

        [Fact]
        public void CustomVirtualProperty_ShouldThrow_WhenUpdatedWithNull()
        {
            // Arrange
            var property = VirtualDummyProperty<string>.Unresolved(x => x.Name);

            // Act
            Action act = () => property.Update(null!);

            // Assert
            act.Should().Throw<VirtualPropertyValueException>()
               .WithMessage("*null value is not allowed*");
        }

        [Fact]
        public void CustomVirtualProperty_ShouldFail_WhenConstructorsArePublic()
        {
            // Act
            Action act = () => InvalidVirtualDummyProperty.Unresolved(x => x.Age);

            // Assert
            act.Should()
               .Throw<VirtualPropertyConstructorResolutionException>()
               .Where(ex => ex.VirtualPropertyType == typeof(InvalidVirtualDummyProperty))
               .WithMessage("*constructor*");
        }

        [Fact]
        public void CustomVirtualProperty_ShouldFail_WhenConstructorsAreMissing()
        {
            // Act
            Action act = () => BrokenVirtualDummyProperty<int>.Unresolved(x => x.Age);

            // Assert
            act.Should()
               .Throw<VirtualPropertyConstructorResolutionException>()
               .Where(ex => ex.VirtualPropertyType == typeof(BrokenVirtualDummyProperty<int>))
               .WithMessage("*constructor*");
        }

    }
}

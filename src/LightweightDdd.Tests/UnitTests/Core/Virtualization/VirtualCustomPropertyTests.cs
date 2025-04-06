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
        public void CustomVirtualProperty_ShouldResolveAndReturnValue()
        {
            // Arrange
            var expected = 42;
            var property = VirtualDummyProperty<int>.Unresolved(x => x.Age);

            // Act
            var resolved = property.Resolve(expected);

            // Assert
            resolved.GetValueOrThrow().Should().Be(expected);
        }

        [Fact]
        public void CustomNullableVirtualProperty_ShouldReturnNull_WhenResolvedWithNull()
        {
            // Arrange
            var property = NullableVirtualDummyProperty<string>.Unresolved(x => x.OptionalName);

            // Act
            var resolved = property.Resolve(null);

            // Assert
            resolved.GetValueOrThrow().Should().BeNull();
        }

        [Fact]
        public void CustomNullableVirtualProperty_ShouldReturnValue_WhenResolved()
        {
            // Arrange
            var expected = "optional";
            var property = NullableVirtualDummyProperty<string>.Unresolved(x => x.OptionalName);

            // Act
            var resolved = property.Resolve(expected);

            // Assert
            resolved.GetValueOrThrow().Should().Be(expected);
        }

        [Fact]
        public void CustomVirtualProperty_ShouldThrow_WhenResolvedWithNull()
        {
            // Arrange
            var property = VirtualDummyProperty<string>.Unresolved(x => x.Name);

            // Act
            Action act = () => property.Resolve(null!);

            // Assert
            act.Should().Throw<VirtualPropertyValueException>()
                .WithMessage($"Null value is not allowed for virtual property '{property.PropertyName}' on entity '{property.EntityName}'.");
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

// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using FluentAssertions;
using LightweightDdd.Core.Extensions;
using LightweightDdd.Core.Virtualization.Exceptions;
using LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers;
using System.Reflection;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization
{
    public sealed class VirtualCustomPropertyTests
    {
        [Fact]
        public void CustomVirtualProperty_ShouldResolveAndReturnValue()
        {
            // Arrange
            var expected = 42;
            var property = VirtualDummyProperty<int>.CreateFor(x => x.Age);

            // Act
            var resolved = property.Resolve(expected);

            // Assert
            resolved.GetValueOrThrow().Should().Be(expected);
        }

        [Fact]
        public void CustomNullableVirtualProperty_ShouldReturnNull_WhenResolvedWithNull()
        {
            // Arrange
            var property = NullableVirtualDummyProperty<string>.CreateFor(x => x.OptionalName);

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
            var property = NullableVirtualDummyProperty<string>.CreateFor(x => x.OptionalName);

            // Act
            var resolved = property.Resolve(expected);

            // Assert
            resolved.GetValueOrThrow().Should().Be(expected);
        }

        [Fact]
        public void CustomVirtualProperty_ShouldThrow_WhenResolvedWithNull()
        {
            // Arrange
            var property = VirtualDummyProperty<string>.CreateFor(x => x.Name);

            // Act
            Action act = () => property.Resolve(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("value");
        }

        [Fact]
        public void CustomVirtualProperty_ShouldFail_WhenConstructorsArePublic()
        {
            // Act
            Action act = () => InvalidVirtualDummyProperty.CreateFor(x => x.Age);

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
            Action act = () => BrokenVirtualDummyProperty<int>.CreateFor(x => x.Age);

            // Assert
            act.Should()
               .Throw<VirtualPropertyConstructorResolutionException>()
               .Where(ex => ex.VirtualPropertyType == typeof(BrokenVirtualDummyProperty<int>))
               .WithMessage("*constructor*");
        }

    }
}

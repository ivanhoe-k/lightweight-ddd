// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using FluentAssertions;
using LightweightDdd.Domain.Virtualization.Exceptions;
using LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization
{
    public class VirtualArgsBuilderTests
    {
        [Fact]
        public void ResolveProperty_ShouldHydrateValue()
        {
            // Arrange
            var builder = DummyEntityArgs.GetBuilder();

            // Act
            var result = builder.WithAge(123).Build();

            // Assert
            result.Age.GetValueOrThrow().Should().Be(123);
            result.Age.HasChanged.Should().BeFalse();
        }

        [Fact]
        public void ResolveProperty_ShouldThrowVirtualPropertyResolutionException_WhenCalledTwice()
        {
            // Arrange
            var builder = DummyEntityArgs.GetBuilder().WithAge(10);

            // Act
            Action act = () => builder.WithAge(20); // Second resolve should fail

            // Assert
            act.Should().Throw<VirtualPropertyResolutionException>()
               .WithMessage("Virtual property 'Age' on entity 'DummyEntity' has already been resolved.");
        }
    }
}

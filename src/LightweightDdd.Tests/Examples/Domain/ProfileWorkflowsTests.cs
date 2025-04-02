// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Contracts;
using LightweightDdd.Examples.Domain.Models.Virtualization;
using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Examples.Domain.Workflows;
using LightweightDdd.Examples.Domain;
using Moq;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using LightweightDdd.Core.Results;

namespace LightweightDdd.Tests.Examples.Domain
{
    public class ProfileWorkflowsTests
    {
        [Fact]
        public async Task ReplaceGallery_ShouldSucceed_WhenGalleryIsValid()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var version = 1L;

            var media1 = Media.Create("https://img1.jpg", "img1.jpg", "image/jpeg").Value;
            var media2 = Media.Create("https://img2.jpg", "img2.jpg", "image/jpeg").Value;
            var freeSubscription = SubscriptionPlan.Free;

            var virtualProfile = Profile
                .CreateVirtual(
                    id: profileId, 
                    version: version,
                    args: VirtualProfileArgs
                        .GetBuilder()
                        .WithGallery(new[] { media1 })
                        .WithSubscription(freeSubscription)
                        .Build()).Value;

            var readRepo = new Mock<IProfileReadOnlyRepository>();
            var writeRepo = new Mock<IProfileWriteOnlyRepository>();
            var logger = new Mock<ILogger<ProfileWorkflows>>();

            readRepo.Setup(r => r.ResolveForGalleryUpdateAsync(profileId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<IDomainError>.Ok(virtualProfile));

            writeRepo.Setup(r => r.UpdateGalleryAsync(It.IsAny<Profile>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<IDomainError>.Ok());

            var workflow = new ProfileWorkflows(logger.Object, readRepo.Object, writeRepo.Object);

            // Act
            var result = await workflow.UpdateGalleryAsync(profileId, new[] { media1, media2 }, CancellationToken.None);

            // Assert
            result.Failed.Should().BeFalse("gallery update should succeed when data is valid");
            result.Value.Should().BeEquivalentTo(new[] { media1, media2 }, "gallery should reflect the updated media items");

            readRepo.Verify(r => r.ResolveForGalleryUpdateAsync(profileId, It.IsAny<CancellationToken>()), Times.Once);
            writeRepo.Verify(r => r.UpdateGalleryAsync(It.IsAny<Profile>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

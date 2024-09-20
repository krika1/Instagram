using AutoMapper;
using Meta.Instagram.Bussines.Services;
using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Meta.Instagram.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace Meta.Instagram.UnitTests.Tests
{
    public class ProfileTests
    {
        private readonly Mock<IProfileRepository> _mockProfileRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProfileService _profileService;

        public ProfileTests()
        {
            _mockProfileRepository = new Mock<IProfileRepository>();
            _mockMapper = new Mock<IMapper>();
            _profileService = new ProfileService(_mockProfileRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task UpdateProfileAsync_ProfileNotFound_ThrowsNotFoundException()
        {
            // Arrange
            string profileId = Guid.NewGuid().ToString();
            var request = new ChangeProfileRequest();

            _mockProfileRepository
                .Setup(repo => repo.GetProfileAsync(profileId))
                .ReturnsAsync((Infrastructure.Entities.Profile)null); // Simulating a not found profile

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _profileService.UpdateProfileAsync(profileId, request));
        }

        [Fact]
        public async Task UpdateProfileAsync_InvalidFileType_ThrowsBadRequestException()
        {
            // Arrange
            string profileId = Guid.NewGuid().ToString();
            var request = new ChangeProfileRequest
            {
                Picture = new FormFile(new MemoryStream(new byte[0]), 0, 0, "file", "file.txt") // Invalid file type
            };

            var profile = new Infrastructure.Entities.Profile { PicturePath = "oldPath" };
            _mockProfileRepository
                .Setup(repo => repo.GetProfileAsync(profileId))
                .ReturnsAsync(profile);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _profileService.UpdateProfileAsync(profileId, request));
        }

        [Fact]
        public async Task UpdateProfileAsync_ValidRequest_UpdatesProfileSuccessfully()
        {
            // Arrange
            string profileId = Guid.NewGuid().ToString();
            var request = new ChangeProfileRequest
            {
                Description = "New Description"
            };

            var profile = new Infrastructure.Entities.Profile { PicturePath = "oldPath", Description = "Old Description" };
            var updatedProfile = new Infrastructure.Entities.Profile { PicturePath = "newPath", Description = "New Description" };
            var contract = new ProfileContract { PicturePath = "newPath", Description = "New Description" };

            _mockProfileRepository
                .Setup(repo => repo.GetProfileAsync(profileId))
                .ReturnsAsync(profile);

            _mockProfileRepository
                .Setup(repo => repo.UpdateProfileAsync(It.IsAny<Infrastructure.Entities.Profile>()))
                .ReturnsAsync(updatedProfile);

            _mockMapper
                .Setup(mapper => mapper.Map<ProfileContract>(updatedProfile))
                .Returns(contract);

            // Act
            var result = await _profileService.UpdateProfileAsync(profileId, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contract.Description, result.Description);
            _mockProfileRepository.Verify(repo => repo.UpdateProfileAsync(It.IsAny<Infrastructure.Entities.Profile>()), Times.Once);
        }

        [Fact]
        public async Task GetProfileAsync_ProfileNotFound_ThrowsNotFoundException()
        {
            // Arrange
            string profileId = Guid.NewGuid().ToString();

            _mockProfileRepository
                .Setup(repo => repo.GetProfileAsync(profileId))
                .ReturnsAsync((Infrastructure.Entities.Profile)null); // Simulating a not found profile

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _profileService.GetProfileAsync(profileId));
            Assert.Equal(ErrorMessages.ProfileNotFoundErrorMessage, exception.Message);
        }

        [Fact]
        public async Task GetProfileAsync_ProfileFound_ReturnsProfileContract()
        {
            // Arrange
            string profileId = Guid.NewGuid().ToString();
            var profile = new Infrastructure.Entities.Profile
            {
                ProfileId = profileId,
                Username = "Test Profile",
                PicturePath = "path/to/picture.jpg",
                Description = "Profile Description"
            };
            var profileContract = new ProfileContract
            {
                ProfileId = profileId,
                Username = "Test Profile",
                PicturePath = "path/to/picture.jpg",
                Description = "Profile Description"
            };

            _mockProfileRepository
                .Setup(repo => repo.GetProfileAsync(profileId))
                .ReturnsAsync(profile); // Simulating a found profile

            _mockMapper
                .Setup(mapper => mapper.Map<ProfileContract>(profile))
                .Returns(profileContract); // Simulating mapping behavior

            // Act
            var result = await _profileService.GetProfileAsync(profileId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(profileContract.ProfileId, result.ProfileId);
            Assert.Equal(profileContract.Username, result.Username);
            Assert.Equal(profileContract.PicturePath, result.PicturePath);
            Assert.Equal(profileContract.Description, result.Description);

            _mockProfileRepository.Verify(repo => repo.GetProfileAsync(profileId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<ProfileContract>(profile), Times.Once);
        }
    }
}


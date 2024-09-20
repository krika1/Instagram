using AutoMapper;
using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Meta.Instagram.Infrastructure.Helpers;
using Meta.Instagram.Infrastructure.Repositories;
using Meta.Instagram.Infrastructure.Services;
using Profile = Meta.Instagram.Infrastructure.Entities.Profile;

namespace Meta.Instagram.Bussines.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        public ProfileService(IProfileRepository profileRepository, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
        }

        public async Task FollowProfileAsync(string followingId, FollowRequest request)
        {
            await GetProfile(followingId).ConfigureAwait(false);
            await GetProfile(request.FollowerId!).ConfigureAwait(false);

            var followRequest = new Follow { FollowerId =  request.FollowerId, FollowingId = followingId };

            await _profileRepository.FollowProfileAsync(followRequest).ConfigureAwait(false);
        }

        public async Task<ProfileContract> GetProfileAsync(string profileId)
        {
            var profile = await GetProfile(profileId).ConfigureAwait(false);

            return _mapper.Map<ProfileContract>(profile);
        }

        public async Task<ProfileContract> UpdateProfileAsync(string profileId, ChangeProfileRequest request)
        {
            var profile = await GetProfile(profileId).ConfigureAwait(false);

            if (request.Picture is not null)
            {
                var fileExtension = Path.GetExtension(request.Picture.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new BadRequestException(ErrorMessages.InvalidFileTypeErrorMessage);
                }

                var fileName = Guid.NewGuid() + fileExtension; // Unique file name
                var filePath = Path.Combine(Constants.BlobPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.Picture.CopyToAsync(stream);

                profile.PicturePath = filePath;
            }
            if (request.Description is not null)
            {
                profile.Description = request.Description;
            }
            if (request.IsPublic.HasValue)
            {
                profile.IsPublic = request.IsPublic.Value;
            }

            profile.UpdatedAt = DateTime.Now;

            var updatedProfile = await _profileRepository.UpdateProfileAsync(profile).ConfigureAwait(false);

            return _mapper.Map<ProfileContract>(updatedProfile);
        }

        public async Task<Profile> GetProfile(string profileId)
        {
            return await _profileRepository.GetProfileAsync(profileId).ConfigureAwait(false)
                   ?? throw new NotFoundException(ErrorMessages.ProfileNotFoundErrorMessage);
        }
    }
}

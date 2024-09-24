using AutoMapper;
using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Meta.Instagram.Infrastructure.Helpers;
using Meta.Instagram.Infrastructure.Repositories;
using Meta.Instagram.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;
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
            await GetProfile(request.FollowerId!).ConfigureAwait(false);
            await GetProfile(followingId).ConfigureAwait(false);

            var followRequest = new Follow { FollowerId =  request.FollowerId!, FollowingId = followingId };

            await _profileRepository.FollowProfileAsync(followRequest).ConfigureAwait(false);
        }

        public async Task UnFollowProfileAsync(string followingId, FollowRequest request)
        {
            await GetProfile(followingId).ConfigureAwait(false);
            var follower = await GetProfile(request.FollowerId!).ConfigureAwait(false);

            var followRequest = follower.Following!.Where(x => x.FollowingId == followingId).FirstOrDefault();

            await _profileRepository.UnFollowProfileAsync(followRequest!).ConfigureAwait(false);
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
                var filePath = await SavePictureAsync(request.Picture).ConfigureAwait(false);

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

        public async Task<IEnumerable<ProfileFollowContract>> GetProfileFollowersAsync(string profileId)
        {
            var profile = await GetProfile(profileId).ConfigureAwait(false);

            var followers = new List<ProfileFollowContract>();

            foreach (var follower in profile.Followers!)
            {
                var follow = _mapper.Map<ProfileFollowContract>(follower.Follower);

                followers.Add(follow);
            }

            return followers;
        }

        public async Task<IEnumerable<ProfileFollowContract>> GetProfileFollowingAsync(string profileId)
        {
            var profile = await GetProfile(profileId).ConfigureAwait(false);

            var followings = new List<ProfileFollowContract>();

            foreach (var following in profile.Following!)
            {
                var follow = _mapper.Map<ProfileFollowContract>(following.Following);

                followings.Add(follow);
            }

            return followings;
        }

        public async Task UploadPictureAsync(string profileId, UploadPictureRequest request)
        {
            var profile = await GetProfile(profileId).ConfigureAwait(false);

            var filePath = await SavePictureAsync(request.Picture!).ConfigureAwait(false);

            var picture = _mapper.Map<Picture>(request);
            picture.ProfileId = profileId;
            picture.PicturePath = filePath;

            await _profileRepository.UploadPictureAsync(profile, picture).ConfigureAwait(false);
        }

        private async Task<Profile> GetProfile(string profileId)
        {
            return await _profileRepository.GetProfileAsync(profileId).ConfigureAwait(false)
                   ?? throw new NotFoundException(ErrorMessages.ProfileNotFoundErrorMessage);
        }

        private async Task<string> SavePictureAsync(IFormFile picture)
        {
            var fileExtension = Path.GetExtension(picture.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new BadRequestException(ErrorMessages.InvalidFileTypeErrorMessage);
            }

            var fileName = Guid.NewGuid() + fileExtension; // Unique file name
            var filePath = Path.Combine(Constants.BlobPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await picture.CopyToAsync(stream);

            return filePath;
        }
    }
}

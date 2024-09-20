using AutoMapper;
using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Meta.Instagram.Infrastructure.Helpers;
using Meta.Instagram.Infrastructure.Repositories;
using Meta.Instagram.Infrastructure.Services;

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

        public async Task<ProfileContract> UpdateProfileAsync(string profileId, ChangeProfileRequest request)
        {
            var profile = await _profileRepository.GetProfileAsync(profileId).ConfigureAwait(false)
                    ?? throw new NotFoundException(ErrorMessages.ProfileNotFoundErrorMessage);

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

            var updatedProfile = await _profileRepository.UpdateProfileAsync(profile).ConfigureAwait(false);

            return _mapper.Map<ProfileContract>(updatedProfile);
        }
    }
}

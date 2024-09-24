using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Repositories;
using Meta.Instagram.Infrastructure.Services;
using AutoMapper;
using System;
using Meta.Instagram.Infrastructure.Helpers;

namespace Meta.Instagram.Bussines.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;

        public PictureService(IPictureRepository pictureRepository, IMapper mapper)
        {
            _pictureRepository = pictureRepository;
            _mapper = mapper;
        }

        public async Task DeletePictureAsync(string pictureId)
        {
            var picture = await _pictureRepository.GetPictureAsync(pictureId).ConfigureAwait(false)
                 ?? throw new NotFoundException(ErrorMessages.PictureNotFoundErrorMessage);

            var filePath = Path.Combine(Constants.BlobPath, picture.PicturePath!);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await _pictureRepository.DeletePictureAsync(picture).ConfigureAwait(false);
        }

        public async Task<PictureContract> GetPictureAsync(string pictureId)
        {
            var picture = await _pictureRepository.GetPictureAsync(pictureId).ConfigureAwait(false)
                  ?? throw new NotFoundException(ErrorMessages.PictureNotFoundErrorMessage);

            return _mapper.Map<PictureContract>(picture);
        }
    }
}

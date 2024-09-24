using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.Exceptions.Errors;
using Meta.Instagram.Infrastructure.Exceptions;
using Meta.Instagram.Infrastructure.Repositories;
using Meta.Instagram.Infrastructure.Services;
using AutoMapper;
using System;
using Meta.Instagram.Infrastructure.Helpers;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Entities;

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
            var picture = await GetPicture(pictureId).ConfigureAwait(false);

            var filePath = Path.Combine(Constants.BlobPath, picture.PicturePath!);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await _pictureRepository.DeletePictureAsync(picture).ConfigureAwait(false);
        }

        public async Task<PictureContract> GetPictureAsync(string pictureId)
        {
            var picture = await GetPicture(pictureId).ConfigureAwait(false);

            return _mapper.Map<PictureContract>(picture);
        }

        public async Task<PictureContract> LikePictureAsync(string pictureId, LikeRequest request)
        {
            var picture = await GetPicture(pictureId).ConfigureAwait(false);

            var like = _mapper.Map<Like>(request);
            like.PictureId = pictureId;
      
            var likedPicture = await _pictureRepository.LikePictureAsync(picture, like).ConfigureAwait(false);

            return _mapper.Map<PictureContract>(likedPicture);
        }

        private async Task<Picture> GetPicture(string pictureId)
        {
            return await _pictureRepository.GetPictureAsync(pictureId).ConfigureAwait(false)
                 ?? throw new NotFoundException(ErrorMessages.PictureNotFoundErrorMessage);
        }
    }
}

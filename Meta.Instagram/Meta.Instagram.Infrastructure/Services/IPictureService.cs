using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;

namespace Meta.Instagram.Infrastructure.Services
{
    public interface IPictureService
    {
        Task<PictureContract> GetPictureAsync(string pictureId);
        Task<PictureContract> LikePictureAsync(string pictureId, LikeRequest request);
        Task DeletePictureAsync(string pictureId);
    }
}

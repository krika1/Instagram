using Meta.Instagram.Infrastructure.DTOs.Contracts;

namespace Meta.Instagram.Infrastructure.Services
{
    public interface IPictureService
    {
        Task<PictureContract> GetPictureAsync(string pictureId);
        Task DeletePictureAsync(string pictureId);
    }
}

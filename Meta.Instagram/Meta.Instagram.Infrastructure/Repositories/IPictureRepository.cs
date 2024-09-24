using Meta.Instagram.Infrastructure.Entities;

namespace Meta.Instagram.Infrastructure.Repositories
{
    public interface IPictureRepository
    {
        Task<Picture> GetPictureAsync(string pictureId);
        Task DeletePictureAsync(Picture picture);
    }
}

using Meta.Instagram.Infrastructure.Entities;

namespace Meta.Instagram.Infrastructure.Repositories
{
    public interface IProfileRepository
    {
        Task CreateProfileAsync(Profile profile);
    }
}

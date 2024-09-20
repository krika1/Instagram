using Meta.Instagram.Infrastructure.Entities;

namespace Meta.Instagram.Infrastructure.Repositories
{
    public interface IProfileRepository
    {
        Task CreateProfileAsync(Profile profile);
        Task DeleteProfileAsync(Profile profile);
        Task<Profile> GetProfileAsync(string profileId);
        Task<Profile> UpdateProfileAsync(Profile profile);
    }
}

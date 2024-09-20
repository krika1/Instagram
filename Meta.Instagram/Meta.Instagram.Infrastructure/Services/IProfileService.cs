using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;

namespace Meta.Instagram.Infrastructure.Services
{
    public interface IProfileService
    {
        Task<ProfileContract> UpdateProfileAsync(string profileId, ChangeProfileRequest request);
        Task<ProfileContract> GetProfileAsync(string profileId);
    }
}

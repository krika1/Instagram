using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;

namespace Meta.Instagram.Infrastructure.Services
{
    public interface IProfileService
    {
        Task<ProfileContract> UpdateProfileAsync(string profileId, ChangeProfileRequest request);
        Task<ProfileContract> GetProfileAsync(string profileId);
        Task<IEnumerable<ProfileFollowContract>> GetProfileFollowersAsync(string profileId);
        Task<IEnumerable<ProfileFollowContract>> GetProfileFollowingAsync(string profileId);
        Task FollowProfileAsync(string followingId, FollowRequest request);
        Task UnFollowProfileAsync(string followingId, FollowRequest request);
    }
}

using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.Entities;

namespace Meta.Instagram.Api.Mapping
{
    public class ProfileProfile : AutoMapper.Profile
    {
        public ProfileProfile()
        {
            _ = CreateMap<Account, Infrastructure.Entities.Profile>()
               .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
               .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));

            _ = CreateMap<Infrastructure.Entities.Profile, ProfileContract>();
        }
    }
}

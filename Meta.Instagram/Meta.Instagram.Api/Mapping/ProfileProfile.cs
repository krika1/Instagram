using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Helpers;

namespace Meta.Instagram.Api.Mapping
{
    public class ProfileProfile : AutoMapper.Profile
    {
        public ProfileProfile()
        {
            _ = CreateMap<Account, Infrastructure.Entities.Profile>()
               .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => IdGenerator.GenerateProfileId()))
               .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
               .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
               .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => true))
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));

            _ = CreateMap<Infrastructure.Entities.Profile, ProfileContract>()
                .ForMember(dest => dest.Following, opt => opt.MapFrom(src => src.Following!.Count))
               .ForMember(dest => dest.Followers, opt => opt.MapFrom(src => src.Followers!.Count));
        }
    }
}

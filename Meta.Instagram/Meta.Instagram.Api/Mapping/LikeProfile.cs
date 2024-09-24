using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Helpers;

namespace Meta.Instagram.Api.Mapping
{
    public class LikeProfile : AutoMapper.Profile
    {
        public LikeProfile()
        {
            _ = CreateMap<LikeRequest, Like>()
               .ForMember(dest => dest.LikeId, opt => opt.MapFrom(src => IdGenerator.GenerateLikeId()))
               .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId));
        }
    }
}

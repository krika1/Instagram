using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.DTOs.Requests;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Helpers;

namespace Meta.Instagram.Api.Mapping
{
    public class PictureProfile : AutoMapper.Profile
    {
        public PictureProfile()
        {
            _ = CreateMap<UploadPictureRequest, Picture>()
                 .ForMember(dest => dest.PictureId, opt => opt.MapFrom(src => IdGenerator.GeneratePictureId()))
                 .ForMember(dest => dest.Descripton, opt => opt.MapFrom(src => src.Description));

            _ = CreateMap<Picture, PictureContract>()
                .ForMember(dest => dest.PictureId, opt => opt.MapFrom(src => src.PictureId))
                .ForMember(dest => dest.Descripton, opt => opt.MapFrom(src => src.Descripton))
                .ForMember(dest => dest.UploadAt, opt => opt.MapFrom(src => src.UploadAt))
                .ForMember(dest => dest.PicturePath, opt => opt.MapFrom(src => src.PicturePath))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Count));
        }
    }
}

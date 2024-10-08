﻿using Auth0.ManagementApi.Models;
using Meta.Instagram.Infrastructure.DTOs.Contracts;
using Meta.Instagram.Infrastructure.Entities;
using Meta.Instagram.Infrastructure.Helpers;
using Profile = AutoMapper.Profile;

namespace Meta.Instagram.Api.Mapping
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            _ = CreateMap<User, Account>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => IdGenerator.GenerateAccountId()))
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            _ = CreateMap<Account, AccountContract>();
        }
    }
}

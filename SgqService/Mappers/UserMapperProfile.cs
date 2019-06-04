﻿using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;
using SgqService.ViewModels;

namespace SgqService.Mappers
{
    public class UserMapperProfile : Profile
    {

        public UserMapperProfile()
        {
            CreateMap<GenericReturn<UserSgq>, GenericReturn<UserViewModel>>();
            CreateMap<UserSgq, UserViewModel>();
            CreateMap<UserSgq, UserDTO>();
            CreateMap<GenericReturn<UserDTO>, GenericReturn<UserViewModel>>();
            CreateMap<UserDTO, UserViewModel>();
            CreateMap<UserViewModel, UserDTO>();
            CreateMap<UserDTO, UserSgq>();
        }

    }
}
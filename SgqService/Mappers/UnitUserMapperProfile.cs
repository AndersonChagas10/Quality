﻿using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqService.Mappers
{
    public class UnitUserMapperProfile : Profile
    {
        public UnitUserMapperProfile()
        {
            CreateMap<UnitUser, UnitUserDTO>();
            CreateMap<UnitUserDTO, UnitUser>();
        }
    }
}
﻿using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqService.Mappers
{
    public class ParCompanyXUserSgqMapperProfile : Profile
    {

        public ParCompanyXUserSgqMapperProfile()
        {
            CreateMap<ParCompanyXUserSgq, ParCompanyXUserSgqDTO>();
            CreateMap<ParCompanyXUserSgqDTO, ParCompanyXUserSgq>();

        }
    }
}
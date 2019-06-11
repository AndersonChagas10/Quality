﻿using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParClusterGroupMapperProfile : Profile
    {
        public ParClusterGroupMapperProfile()
        {
            CreateMap<ParClusterGroupDTO, ParClusterGroup>();
            CreateMap<ParClusterGroup, ParClusterGroupDTO>();
        }
    }
}
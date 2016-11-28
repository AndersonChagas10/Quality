﻿using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class UnitUserMapperProfile : Profile
    {
        public UnitUserMapperProfile()
        {
            CreateMap<ParCompanyStructure, ParCompanyStructureDTO>();
            CreateMap<ParCompanyStructureDTO, ParCompanyStructure>();
        }
    }
}
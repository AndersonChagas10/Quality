﻿using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParDepartmentMapperProfile : Profile
    {
        public ParDepartmentMapperProfile()
        {
            CreateMap<ParDepartment, ParDepartmentDTO>();
            CreateMap<ParDepartmentDTO, ParDepartment>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqService.Mappers
{
    public class DefectMapperProfile : Profile
    {
        public DefectMapperProfile()
        {
            CreateMap<Defect, DefectDTO>();
            CreateMap<DefectDTO, Defect>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Mappers
{
    public class ParClusterXModuleMapperProfile : Profile
    {
        public ParClusterXModuleMapperProfile()
        {
            CreateMap<ParClusterXModule, ParClusterXModulesDTO>();
            CreateMap<ParClusterXModulesDTO, ParClusterXModule>();
        }
    }
}
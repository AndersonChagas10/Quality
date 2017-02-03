using AutoMapper;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Mappers
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            CreateMap<RoleType, RoleTypeDTO>();
            CreateMap<RoleTypeDTO, RoleType>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Mappers
{
    public class RoleUserSgqXItemMenuMapperProfile : Profile
    {

        public RoleUserSgqXItemMenuMapperProfile()
        {
            CreateMap<RoleUserSgqXItemMenu, RoleUserSgqXItemMenuDTO>();
            CreateMap<RoleUserSgqXItemMenuDTO, RoleUserSgqXItemMenu>();
        }

    }
}
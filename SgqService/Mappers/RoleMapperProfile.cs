﻿using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqService.Mappers
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            CreateMap<RoleType, RoleTypeDTO>();
            CreateMap<RoleTypeDTO, RoleType>();

            CreateMap<ScreenComponent, ScreenComponentDTO>();
            CreateMap<ScreenComponentDTO, ScreenComponent>();

            CreateMap<RoleSGQ, RoleSGQDTO>();
            CreateMap<RoleSGQDTO, RoleSGQ>();

            CreateMap<RoleJBS, RoleJBSDTO>();
            CreateMap<RoleJBSDTO, RoleJBS>();
        }
    }
}
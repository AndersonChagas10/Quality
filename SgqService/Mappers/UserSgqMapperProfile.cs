using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqService.Mappers
{
    public class UserSgqMapperProfile : Profile
    {
        public UserSgqMapperProfile()
        {
            CreateMap<UserSgq, UserSgqDTO>();
            CreateMap<UserSgqDTO, UserSgq>();
        }
    }
}
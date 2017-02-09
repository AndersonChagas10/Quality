using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Mappers
{
    public class UserSgqProfile : Profile
    {
        public UserSgqProfile()
        {
            CreateMap<UserSgq, UserSgqDTO>();
            CreateMap<UserSgqDTO, UserSgq>();
        }
    }
}
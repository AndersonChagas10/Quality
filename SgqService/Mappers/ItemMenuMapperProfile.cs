using AutoMapper;
using Dominio;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqService.Mappers
{
    public class ItemMenuMapperProfile : Profile
    {
        public ItemMenuMapperProfile()
        {
            CreateMap<ItemMenu, ItemMenuDTO>();
            CreateMap<ItemMenuDTO, ItemMenu>();
        }
    }
}
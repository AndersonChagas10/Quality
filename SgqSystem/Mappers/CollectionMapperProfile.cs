using AutoMapper;
using Dominio;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.Mappers
{
    public class CollectionMapperProfile : Profile
    {
        public CollectionMapperProfile()
        {
            CreateMap<CollectionLevel2, CollectionViewModel>();
            CreateMap<CollectionViewModel, CollectionLevel2>();
        }
    }
}
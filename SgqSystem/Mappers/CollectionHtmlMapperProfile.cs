using AutoMapper;
using Dominio;
using DTO.DTO;
using System.Collections.Generic;

namespace SgqSystem.Mappers
{
    public class CollectionHtmlMapperProfile : Profile
    {
        public CollectionHtmlMapperProfile()
        {
            CreateMap<CollectionHtmlDTO, CollectionHtml>();
            CreateMap<CollectionHtml, CollectionHtmlDTO>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqService.Mappers
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
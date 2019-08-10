using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqServiceBusiness.Mappers
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
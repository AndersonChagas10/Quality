using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqServiceBusiness.Mappers
{
    public class ComponenteGenericoMapperProfile : Profile
    {
        public ComponenteGenericoMapperProfile()
        {
            CreateMap<ComponenteGenericoDTO, ComponenteGenerico>();
            CreateMap<ComponenteGenerico, ComponenteGenericoDTO>();
        }
    }
}
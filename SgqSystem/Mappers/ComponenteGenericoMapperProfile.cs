using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
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
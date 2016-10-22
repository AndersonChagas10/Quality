using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel1HeaderFieldMapperProfile : Profile
    {
        public ParLevel1HeaderFieldMapperProfile()
        {
            CreateMap<ParLevel1XHeaderFieldDTO, ParLevel1HeaderField>();
            CreateMap<ParLevel1HeaderField, ParLevel1XHeaderFieldDTO>();
        }
    }
}
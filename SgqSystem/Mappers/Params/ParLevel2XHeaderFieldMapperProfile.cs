using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel2XHeaderFieldMapperProfile : Profile
    {
        public ParLevel2XHeaderFieldMapperProfile()
        {
            CreateMap<ParLevel2XHeaderField, ParLevel2XHeaderFieldDTO>();
            CreateMap<ParLevel2XHeaderFieldDTO, ParLevel2XHeaderField>();
        }
    }
}

using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel1XHeaderFieldMapperProfile : Profile
    {
        public ParLevel1XHeaderFieldMapperProfile()
        {
            CreateMap<ParLevel1XHeaderField, ParLevel1XHeaderFieldDTO>();
            CreateMap<ParLevel1XHeaderFieldDTO, ParLevel1XHeaderField>();
        }
    }
}

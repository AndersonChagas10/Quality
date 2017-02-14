using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel3BoolTrueMapperProfile : Profile
    {
        public ParLevel3BoolTrueMapperProfile()
        {
            CreateMap<ParLevel3BoolTrue, ParLevel3BoolTrueDTO>();
            CreateMap<ParLevel3BoolTrueDTO, ParLevel3BoolTrue>();
        }
    }
}
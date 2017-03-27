using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParScoreTypeMapperProfile : Profile
    {
        public ParScoreTypeMapperProfile()
        {
            CreateMap<ParScoreType, ParScoreTypeDTO>();
            CreateMap<ParScoreTypeDTO, ParScoreType>();
        }
    }
}
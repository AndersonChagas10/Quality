using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel3inputTypeMapperProfile : Profile
    {
        public ParLevel3inputTypeMapperProfile()
        {
            CreateMap<ParLevel3InputType, ParLevel3InputTypeDTO>();
            CreateMap<ParLevel3InputTypeDTO, ParLevel3InputType>();
        }
    }
}
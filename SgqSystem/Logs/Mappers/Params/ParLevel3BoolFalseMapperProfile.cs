using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{

    public class ParLevel3BoolFalseMapperProfile : Profile
    {
        public ParLevel3BoolFalseMapperProfile()
        {
            CreateMap<ParLevel3BoolFalse, ParLevel3BoolFalseDTO>();
            CreateMap<ParLevel3BoolFalseDTO, ParLevel3BoolFalse>();
        }
    }
}
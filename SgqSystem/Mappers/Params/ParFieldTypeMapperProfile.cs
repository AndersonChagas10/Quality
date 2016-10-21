using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParFieldTypeMapperProfile : Profile
    {
        public ParFieldTypeMapperProfile()
        {
            CreateMap<ParFieldTypeDTO, ParFieldType>();
            CreateMap<ParFieldType, ParFieldTypeDTO>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
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
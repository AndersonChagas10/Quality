using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParStructureMapperProfile : Profile
    {
        public ParStructureMapperProfile()
        {
            CreateMap<ParStructureDTO, ParStructure>();
            CreateMap<ParStructure, ParStructureDTO>();
        }
    }
}
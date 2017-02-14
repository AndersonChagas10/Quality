using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
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
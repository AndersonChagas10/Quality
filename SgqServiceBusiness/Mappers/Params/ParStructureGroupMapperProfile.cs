using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParStructureGroupMapperProfile : Profile
    {
        public ParStructureGroupMapperProfile()
        {
            CreateMap<ParStructureGroupDTO, ParStructureGroup>();
            CreateMap<ParStructureGroup, ParStructureGroupDTO>();
        }
    }
}
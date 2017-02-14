using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParConsolidationTypeMapperProfile : Profile
    {
        public ParConsolidationTypeMapperProfile()
        {
            CreateMap<ParConsolidationType, ParConsolidationTypeDTO>();
            CreateMap<ParConsolidationTypeDTO, ParConsolidationType>();
        }
    }
}
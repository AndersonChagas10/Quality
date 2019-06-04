using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqService.Mappers
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
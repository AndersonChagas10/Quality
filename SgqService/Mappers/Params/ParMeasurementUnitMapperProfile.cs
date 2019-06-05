using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParMeasurementUnitMapperProfile : Profile
    {
        public ParMeasurementUnitMapperProfile()
        {
            CreateMap<ParMeasurementUnit, ParMeasurementUnitDTO>();
            CreateMap<ParMeasurementUnitDTO, ParMeasurementUnit>();
        }
    }
}
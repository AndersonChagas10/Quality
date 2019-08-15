using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
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
using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParMultipleValuesMapperProfile : Profile
    {
        public ParMultipleValuesMapperProfile()
        {
            CreateMap<ParMultipleValuesDTO, ParMultipleValues>();
            CreateMap<ParMultipleValues, ParMultipleValuesDTO>();
        }
    }
}
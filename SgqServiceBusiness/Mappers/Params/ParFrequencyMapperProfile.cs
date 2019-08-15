using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParFrequencyMapperProfile : Profile
    {
        public ParFrequencyMapperProfile()
        {
            CreateMap<ParFrequency, ParFrequencyDTO>();
            CreateMap<ParFrequencyDTO, ParFrequency>();
        }
    }
}
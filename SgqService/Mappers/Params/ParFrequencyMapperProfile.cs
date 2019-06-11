using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqService.Mappers
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
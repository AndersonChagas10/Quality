using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
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
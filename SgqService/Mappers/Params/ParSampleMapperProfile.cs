using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParSampleMapperProfile : Profile
    {
        public ParSampleMapperProfile()
        {
            CreateMap<ParSample, ParSampleDTO>();
            CreateMap<ParSampleDTO, ParSample>();
        }
    }
}
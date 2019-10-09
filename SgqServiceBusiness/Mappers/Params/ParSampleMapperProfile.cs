using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
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
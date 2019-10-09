using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParLevelDefinitonMapperProfile : Profile
    {
        public ParLevelDefinitonMapperProfile()
        {
            CreateMap<ParLevelDefiniton, ParLevelDefinitonDTO>();
            CreateMap<ParLevelDefinitonDTO, ParLevelDefiniton>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParCounterMapperProfile : Profile
    {
        public ParCounterMapperProfile()
        {
            CreateMap<ParCounter, ParCounterDTO>();
            CreateMap<ParCounterDTO, ParCounter>();
        }
    }
}
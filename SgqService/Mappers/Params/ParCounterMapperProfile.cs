using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
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
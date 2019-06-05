using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParCounterLocalMapperProfile : Profile
    {
        public ParCounterLocalMapperProfile()
        {
            CreateMap<ParCounterXLocal, ParCounterXLocalDTO>();
            CreateMap<ParCounterXLocalDTO, ParCounterXLocal>();
        }
    }
}
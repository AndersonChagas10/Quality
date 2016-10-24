using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParCounterLocalMapperProfile : Profile
    {
        public ParCounterLocalMapperProfile()
        {
            CreateMap<ParCounterLocal, ParCounterLocalDTO>();
            CreateMap<ParCounterLocalDTO, ParCounterLocal>();
        }
    }
}
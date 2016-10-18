using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel1MapperProfile : Profile
    {
        public ParLevel1MapperProfile()
        {
            CreateMap<ParLevel1, ParLevel1DTO>();
            CreateMap<ParLevel1DTO, ParLevel1>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel2Level1MapperProfile : Profile
    {
        public ParLevel2Level1MapperProfile()
        {
            CreateMap<ParLevel2Level1DTO, ParLevel2Level1>();
            CreateMap<ParLevel2Level1, ParLevel2Level1DTO>();
        }
    }
}
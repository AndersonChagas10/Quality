using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel3Level2Level1MapperProfile : Profile
    {
        public ParLevel3Level2Level1MapperProfile()
        {
            CreateMap<ParLevel3Level2Level1, ParLevel3Level2Level1DTO>();
            CreateMap<ParLevel3Level2Level1DTO, ParLevel3Level2Level1>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel3MapperProfile : Profile
    {
        public ParLevel3MapperProfile()
        {
            CreateMap<ParLevel3, ParLevel3DTO>();
            CreateMap<ParLevel3DTO, ParLevel3>();
        }
    }
}
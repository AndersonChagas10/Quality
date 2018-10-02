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

            CreateMap<ParLevel3Value_Outer, ParLevel3Value_OuterListDTO>();
            CreateMap<ParLevel3Value_OuterListDTO, ParLevel3Value_Outer>();
        }
    }
}
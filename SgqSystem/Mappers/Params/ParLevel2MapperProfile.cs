using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel2MapperProfile : Profile
    {
        public ParLevel2MapperProfile()
        {
            CreateMap<ParLevel2, ParLevel2DTO>();
            CreateMap<ParLevel2DTO, ParLevel2>();
        }
    }
}
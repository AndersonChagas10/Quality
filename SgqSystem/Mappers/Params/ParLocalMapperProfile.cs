using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLocalMapperProfile : Profile
    {
        public ParLocalMapperProfile()
        {
            CreateMap<ParLocal, ParLocalDTO>();
            CreateMap<ParLocalDTO, ParLocal>();
        }
    }
}
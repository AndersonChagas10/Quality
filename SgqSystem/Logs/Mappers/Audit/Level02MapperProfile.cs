using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class Level02MapperProfile : Profile
    {
        public Level02MapperProfile()
        {
            CreateMap<Level02, Level02DTO>();
            CreateMap<Level02DTO, Level02>();
        }
    }
}
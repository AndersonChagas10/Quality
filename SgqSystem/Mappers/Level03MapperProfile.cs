using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;

namespace SgqSystem.Mappers
{
    public class Level03MapperProfile : Profile
    {
        public Level03MapperProfile()
        {
            CreateMap<Level03, Level03DTO>();
            CreateMap<Level03DTO, Level03>();
        }
    }
}
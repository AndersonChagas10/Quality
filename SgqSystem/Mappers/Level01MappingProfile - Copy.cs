using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class Level01MappingProfile : Profile
    {
        public Level01MappingProfile()
        {
            CreateMap<Level01, Level01DTO>();
            CreateMap<Level01DTO, Level01>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class Level01MapperProfile : Profile
    {
        public Level01MapperProfile()
        {
            CreateMap<Level01, Level01DTO>();
            CreateMap<Level01DTO, Level01>();
        }
    }
}
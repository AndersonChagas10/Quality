using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class UnitMapperProfile : Profile
    {
        public UnitMapperProfile()
        {
            CreateMap<Unit, UnitDTO>();
            CreateMap<UnitDTO, Unit>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqService.Mappers
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
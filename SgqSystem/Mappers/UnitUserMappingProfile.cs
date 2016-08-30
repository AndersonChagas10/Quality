using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class UnitUserMappingProfile : Profile
    {
        public UnitUserMappingProfile()
        {
            CreateMap<UnitUser, UnitUserDTO>();
            CreateMap<UnitUserDTO, UnitUser>();
        }
    }
}
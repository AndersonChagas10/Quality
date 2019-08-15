using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqServiceBusiness.Mappers
{
    public class RoleUserSgqMapperProfile : Profile
    {
        public RoleUserSgqMapperProfile()
        {
            CreateMap<RoleUserSgq, RoleUserSgqDTO>();
            CreateMap<RoleUserSgqDTO, RoleUserSgq>();
        }
    }
}
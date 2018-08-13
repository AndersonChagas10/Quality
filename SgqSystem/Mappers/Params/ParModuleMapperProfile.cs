using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParModuleMapperProfile : Profile
    {
        public ParModuleMapperProfile()
        {
            CreateMap<ParModuleDTO, ParModule>();
            CreateMap<ParModule, ParModuleDTO>();
        }
    }
}
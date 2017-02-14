using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParCriticalLevelMapperProfile : Profile
    {
        public ParCriticalLevelMapperProfile()
        {
            CreateMap<ParCriticalLevel, ParCriticalLevelDTO>();
            CreateMap<ParCriticalLevelDTO, ParCriticalLevel>();
        }
    }
}
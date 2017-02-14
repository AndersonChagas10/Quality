using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel3ValueMapperProfile : Profile
    {
        public ParLevel3ValueMapperProfile()
        {
            CreateMap<ParLevel3Value, ParLevel3ValueDTO>();
            CreateMap<ParLevel3ValueDTO, ParLevel3Value>();
        }
    }
}
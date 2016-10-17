using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParHeaderFieldMapperProfile : Profile
    {
        public ParHeaderFieldMapperProfile()
        {
            CreateMap<ParHeaderField, ParHeaderFieldDTO>();
            CreateMap<ParHeaderFieldDTO, ParHeaderField>();
        }
    }
}
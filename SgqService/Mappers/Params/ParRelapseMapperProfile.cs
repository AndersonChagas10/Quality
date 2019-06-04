using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParRelapseMapperProfile : Profile
    {
        public ParRelapseMapperProfile()
        {
            CreateMap<ParRelapse, ParRelapseDTO>();
            CreateMap<ParRelapseDTO, ParRelapse>();
        }
    }
}
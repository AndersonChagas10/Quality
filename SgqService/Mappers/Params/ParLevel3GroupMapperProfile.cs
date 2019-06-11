using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParLevel3GroupMapperProfile : Profile
    {
        public ParLevel3GroupMapperProfile()
        {
            CreateMap<ParLevel3Group, ParLevel3GroupDTO>();
            CreateMap<ParLevel3GroupDTO, ParLevel3Group>();
        }
    }
}
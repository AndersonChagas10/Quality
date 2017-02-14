using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel3Level2MapperProfile : Profile
    {
        public ParLevel3Level2MapperProfile()
        {
            CreateMap<ParLevel3Level2, ParLevel3Level2DTO>();
            CreateMap<ParLevel3Level2DTO, ParLevel3Level2>();
        }
    }
}
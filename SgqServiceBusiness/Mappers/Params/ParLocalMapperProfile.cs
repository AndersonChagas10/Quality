using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParLocalMapperProfile : Profile
    {
        public ParLocalMapperProfile()
        {
            CreateMap<ParLocal, ParLocalDTO>();
            CreateMap<ParLocalDTO, ParLocal>();
        }
    }
}
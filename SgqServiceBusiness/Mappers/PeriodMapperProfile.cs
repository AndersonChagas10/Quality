using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqServiceBusiness.Mappers
{
    public class ShiftMapperProfile : Profile
    {
        public ShiftMapperProfile()
        {
            CreateMap<Shift, ShiftDTO>();
            CreateMap<ShiftDTO, Shift>();
        }
    }
}
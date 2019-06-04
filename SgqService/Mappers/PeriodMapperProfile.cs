using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqService.Mappers
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
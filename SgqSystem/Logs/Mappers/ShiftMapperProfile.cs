using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class PeriodMapperProfile : Profile
    {
        public PeriodMapperProfile()
        {
            CreateMap<Period, PeriodDTO>();
            CreateMap<PeriodDTO, Period>();
        }
    }
}
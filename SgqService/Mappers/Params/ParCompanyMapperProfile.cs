using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParCompanyMapperProfile : Profile
    {
        public ParCompanyMapperProfile()
        {
            CreateMap<ParCompany, ParCompanyDTO>();
            CreateMap<ParCompanyDTO, ParCompany>();
        }
    }
}
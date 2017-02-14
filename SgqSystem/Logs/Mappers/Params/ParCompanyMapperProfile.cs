using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
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
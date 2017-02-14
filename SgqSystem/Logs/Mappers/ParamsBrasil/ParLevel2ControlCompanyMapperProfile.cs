using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers.ParamsBrasil
{
    public class ParLevel2ControlCompanyMapperProfile : Profile
    {
        public ParLevel2ControlCompanyMapperProfile()
        {
            CreateMap<ParLevel2ControlCompany, ParLevel2ControlCompanyDTO>();
            CreateMap<ParLevel2ControlCompanyDTO, ParLevel2ControlCompany>();
        }
    }
}
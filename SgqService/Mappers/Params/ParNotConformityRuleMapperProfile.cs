using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParNotConformityRuleMapperProfile : Profile
    {
        public ParNotConformityRuleMapperProfile()
        {
            CreateMap<ParNotConformityRule, ParNotConformityRuleDTO>();
            CreateMap<ParNotConformityRuleDTO, ParNotConformityRule>();
        }
    }
}
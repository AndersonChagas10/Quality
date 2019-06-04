using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParNotConformityRuleXLevelMapperProfile : Profile
    {
        public ParNotConformityRuleXLevelMapperProfile()
        {
            CreateMap<ParNotConformityRuleXLevel, ParNotConformityRuleXLevelDTO>();
            CreateMap<ParNotConformityRuleXLevelDTO, ParNotConformityRuleXLevel>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;

namespace SgqSystem.Mappers
{
    public class Level03ConsolidationMapperProfile : Profile
    {
        public Level03ConsolidationMapperProfile()
        {
            CreateMap<Level03Consolidation, Level03ConsolidationDTO>()
                .ForMember(x => x.Level03, opt => opt.Ignore())
                .ForMember(x => x.Level02Consolidation, opt => opt.Ignore());

            CreateMap<Level03ConsolidationDTO, Level03Consolidation>()
                .ForMember(x => x.Level03, opt => opt.Ignore())
                .ForMember(x => x.Level02Consolidation, opt => opt.Ignore());

            CreateMap<GenericReturn<Level03Consolidation>, GenericReturn<Level03ConsolidationDTO>>();
            CreateMap<GenericReturn<Level03ConsolidationDTO>, GenericReturn<Level03Consolidation>>();
        }
    }
}
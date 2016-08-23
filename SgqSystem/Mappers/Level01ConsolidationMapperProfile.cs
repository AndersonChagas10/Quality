using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;

namespace SgqSystem.Mappers
{
    public class Level01ConsolidationMapperProfile : Profile
    {
        public Level01ConsolidationMapperProfile()
        {
            //CreateMap<Level01Consolidation, Level01ConsolidationDTO>()
            //    .ForMember(x => x.Level01, opt => opt.Ignore())
            //    .ForMember(x => x.Level02Consolidation, opt => opt.Ignore());

            //CreateMap<Level01ConsolidationDTO, Level01Consolidation>()
            //    .ForMember(x => x.Level01, opt => opt.Ignore())
            //    .ForMember(x => x.Level02Consolidation, opt => opt.Ignore());

            //CreateMap<GenericReturn<Level01Consolidation>, GenericReturn<Level01ConsolidationDTO>>();
            //CreateMap<GenericReturn<Level01ConsolidationDTO>, GenericReturn<Level01Consolidation>>();
        }
    }
}
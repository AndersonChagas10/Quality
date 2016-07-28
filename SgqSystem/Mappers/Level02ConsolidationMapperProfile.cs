using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;

namespace SgqSystem.Mappers
{
    public class Level02ConsolidationMapperProfile : Profile
    {
        public Level02ConsolidationMapperProfile()
        {

            CreateMap<Level02Consolidation, Level02ConsolidationDTO>()
                .ForMember(x => x.DataCollection, opt => opt.Ignore())
                .ForMember(x => x.Level01Consolidation, opt => opt.Ignore())
                .ForMember(x => x.Level02, opt => opt.Ignore())
                .ForMember(x => x.Level01Consolidation, opt => opt.Ignore());
            CreateMap<Level02ConsolidationDTO, Level02Consolidation>()
                .ForMember(x => x.DataCollection, opt => opt.Ignore())
                .ForMember(x => x.Level01Consolidation, opt => opt.Ignore())
                .ForMember(x => x.Level02, opt => opt.Ignore())
                .ForMember(x => x.Level01Consolidation, opt => opt.Ignore());

            CreateMap<GenericReturn<Level02Consolidation>, GenericReturn<Level02ConsolidationDTO>>();
            CreateMap<GenericReturn<Level02ConsolidationDTO>, GenericReturn<Level02Consolidation>>();
        }
    }
}
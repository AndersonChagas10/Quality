using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class ConsolidationLevel01MapperProfile : Profile
    {
        public ConsolidationLevel01MapperProfile()
        {
            CreateMap<ConsolidationLevel01DTO, ConsolidationLevel01>();
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
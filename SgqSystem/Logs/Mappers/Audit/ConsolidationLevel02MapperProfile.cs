using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class ConsolidationLevel02MapperProfile : Profile
    {
        public ConsolidationLevel02MapperProfile()
        {

            CreateMap<ConsolidationLevel02DTO, ConsolidationLevel02>();
            CreateMap<ConsolidationLevel02, ConsolidationLevel02DTO>();
            //CreateMap<List<ConsolidationLevel02DTO>, List<ConsolidationLevel02>>();
            //CreateMap<List<ConsolidationLevel02>, List<ConsolidationLevel02DTO>>();

            //    .ForMember(x => x.DataCollection, opt => opt.Ignore())
            //    .ForMember(x => x.Level01Consolidation, opt => opt.Ignore())
            //    .ForMember(x => x.Level02, opt => opt.Ignore())
            //    .ForMember(x => x.Level01Consolidation, opt => opt.Ignore());
            //CreateMap<Level02ConsolidationDTO, Level02Consolidation>()
            //    .ForMember(x => x.DataCollection, opt => opt.Ignore())
            //    .ForMember(x => x.Level01Consolidation, opt => opt.Ignore())
            //    .ForMember(x => x.Level02, opt => opt.Ignore())
            //    .ForMember(x => x.Level01Consolidation, opt => opt.Ignore());

            //CreateMap<GenericReturn<Level02Consolidation>, GenericReturn<Level02ConsolidationDTO>>();
            //CreateMap<GenericReturn<Level02ConsolidationDTO>, GenericReturn<Level02Consolidation>>();
        }
    }
}
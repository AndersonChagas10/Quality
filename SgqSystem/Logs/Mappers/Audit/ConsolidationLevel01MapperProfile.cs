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
            CreateMap<ConsolidationLevel01, ConsolidationLevel01DTO>();//.ForMember(x => x.collectionLevel02DTO, opt => opt.Ignore()).ForMember(x => x.consolidationLevel02DTO, opt => opt.Ignore());
            //CreateMap<List<ConsolidationLevel01>, List<ConsolidationLevel01DTO>>();

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
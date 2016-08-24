using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;

namespace SgqSystem.Mappers
{
    public class CollectionLevel02MapperProfile : Profile
    {
        public CollectionLevel02MapperProfile()
        {
            CreateMap<CollectionLevel02DTO, CollectionLevel02>();
            //    .ForMember(x => x.Level02Consolidation, opt => opt.Ignore())
            //    .ForMember(x => x.DataCollectionResult, opt => opt.Ignore());

            //CreateMap<DataCollection, DataCollectionDTO>()
            //     .ForMember(x => x.Level02Consolidation, opt => opt.Ignore())
            //     .ForMember(x => x.DataCollectionResult, opt => opt.Ignore());

            //CreateMap<GenericReturn<DataCollection>, GenericReturn<DataCollectionDTO>>();

            //CreateMap<GenericReturn<DataCollectionDTO>, GenericReturn<DataCollection>>();
            
                
        }
    }
}
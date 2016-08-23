using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;

namespace SgqSystem.Mappers
{
    public class DataCollectionMapperProfile : Profile
    {
        public DataCollectionMapperProfile()
        {
            //CreateMap<DataCollectionDTO, DataCollection>()
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
using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;

namespace SgqSystem.Mappers
{
    public class DataCollectionResultMapperProfile : Profile
    {
        public DataCollectionResultMapperProfile()
        {
            //CreateMap<DataCollectionResult, DataCollectionResultDTO>()
            //    .ForMember(x => x.Level03, opt => opt.Ignore())
            //    .ForMember(x => x.DataCollection, opt => opt.Ignore());
            //CreateMap<DataCollectionResultDTO, DataCollectionResult>()
            //    .ForMember(x => x.Level03, opt => opt.Ignore())
            //    .ForMember(x => x.DataCollection, opt => opt.Ignore());

            //CreateMap<GenericReturn<DataCollectionResultDTO>, GenericReturn<DataCollectionResult>>();
            //CreateMap<GenericReturn<DataCollectionResult>, GenericReturn<DataCollectionResultDTO>>();
        }
    }
}
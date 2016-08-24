using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.Helpers;

namespace SgqSystem.Mappers
{
    public class CollectionLevel03MapperProfile : Profile
    {
        public CollectionLevel03MapperProfile()
        {
            CreateMap<CollectionLevel03DTO, CollectionLevel03>();
                //.ForMember(x => x.Level03, opt => opt.Ignore())
                //.ForMember(x => x.DataCollection, opt => opt.Ignore());
            //CreateMap<Coll, DataCollectionResult>()
                //.ForMember(x => x.Level03, opt => opt.Ignore())
                //.ForMember(x => x.DataCollection, opt => opt.Ignore());

            //CreateMap<GenericReturn<DataCollectionResultDTO>, GenericReturn<DataCollectionResult>>();
            //CreateMap<GenericReturn<DataCollectionResult>, GenericReturn<DataCollectionResultDTO>>();
        }
    }
}
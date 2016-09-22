using AutoMapper;
using Dominio;
using DTO.DTO;
using System.Collections.Generic;

namespace SgqSystem.Mappers
{
    public class CollectionLevel03MapperProfile : Profile
    {
        public CollectionLevel03MapperProfile()
        {
            CreateMap<CollectionLevel03DTO, CollectionLevel03>();
            CreateMap<CollectionLevel03, CollectionLevel03DTO>();
            //CreateMap<List<CollectionLevel03>, List<CollectionLevel03DTO>>();

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
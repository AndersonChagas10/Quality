using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel1XClusterMapperProfile : Profile
    {
        public ParLevel1XClusterMapperProfile()
        {
            CreateMap<ParLevel1XCluster, ParLevel1XClusterDTO>();
            CreateMap<ParLevel1XClusterDTO, ParLevel1XCluster>();
                    //.ForMember(x => x.ParCluster, opt => opt.Ignore())
                    //.ForMember(x => x.ParLevel1, opt => opt.Ignore());
        }
    }
}
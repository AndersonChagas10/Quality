using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel1ClusterMapperProfile : Profile
    {
        public ParLevel1ClusterMapperProfile()
        {
            CreateMap<ParLevel1XCluster, ParLevel1XClusterDTO>();
            CreateMap<ParLevel1XClusterDTO, ParLevel1XCluster>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel1ClusterMapperProfile : Profile
    {
        public ParLevel1ClusterMapperProfile()
        {
            CreateMap<ParLevel1Cluster, ParLevel1ClusterDTO>();
            CreateMap<ParLevel1ClusterDTO, ParLevel1Cluster>();
        }
    }
}
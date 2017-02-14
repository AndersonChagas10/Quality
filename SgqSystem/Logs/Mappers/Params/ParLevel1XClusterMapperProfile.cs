using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParLevel1XClusterMapperProfile : Profile
    {
        public ParLevel1XClusterMapperProfile()
        {
            CreateMap<ParLevel1XCluster, ParLevel1XClusterDTO>();
            CreateMap<ParLevel1XClusterDTO, ParLevel1XCluster>();
        }
    }
}
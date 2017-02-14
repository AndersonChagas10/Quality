using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParClusterMapperProfile : Profile
    {
        public ParClusterMapperProfile()
        {
            CreateMap<ParCluster, ParClusterDTO>();
            CreateMap<ParClusterDTO, ParCluster>();
        }
    }
}
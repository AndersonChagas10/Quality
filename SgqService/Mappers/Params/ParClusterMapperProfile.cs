using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
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
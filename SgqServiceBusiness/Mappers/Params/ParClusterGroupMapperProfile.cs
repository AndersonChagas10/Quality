using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParClusterGroupMapperProfile : Profile
    {
        public ParClusterGroupMapperProfile()
        {
            CreateMap<ParClusterGroupDTO, ParClusterGroup>();
            CreateMap<ParClusterGroup, ParClusterGroupDTO>();
        }
    }
}
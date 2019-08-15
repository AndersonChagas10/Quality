using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParCompanyClusterProfile : Profile
    {
        public ParCompanyClusterProfile()
        {
            CreateMap<ParCompanyClusterDTO, ParCompanyCluster>();
            CreateMap<ParCompanyCluster, ParCompanyClusterDTO>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParCompanyXStructureMapperProfile : Profile
    {
        public ParCompanyXStructureMapperProfile()
        {
            CreateMap<ParCompanyXStructureDTO, ParCompanyXStructure>();
            CreateMap<ParCompanyXStructure, ParCompanyXStructureDTO>();
        }
    }
}
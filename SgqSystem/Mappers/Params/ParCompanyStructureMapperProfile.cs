using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParCompanyStructureMapperProfile : Profile
    {
        public ParCompanyStructureMapperProfile()
        {
            CreateMap<ParCompanyStructureDTO, ParCompanyStructure>();
            CreateMap<ParCompanyStructure, ParCompanyStructureDTO>();
        }
    }
}
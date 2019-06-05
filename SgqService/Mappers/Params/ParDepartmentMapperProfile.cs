using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqService.Mappers
{
    public class ParDepartmentMapperProfile : Profile
    {
        public ParDepartmentMapperProfile()
        {
            CreateMap<ParDepartment, ParDepartmentDTO>();
            CreateMap<ParDepartmentDTO, ParDepartment>();
        }
    }
}
using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class DepartmenMapperProfile : Profile
    {
        public DepartmenMapperProfile()
        {
            CreateMap<Department, DepartmentDTO>();
            CreateMap<DepartmentDTO, Department>();
        }
    }
}
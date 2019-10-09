using AutoMapper;
using Dominio;
using DTO.DTO;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParLevel3XParDepartmentMapperProfile : Profile
    {
        public ParLevel3XParDepartmentMapperProfile()
        {
            CreateMap<ParLevel3XParDepartment, ParLevel3XDepartmentDTO>();
            CreateMap<ParLevel3XDepartmentDTO, ParLevel3XParDepartment>();
        }
    }
}
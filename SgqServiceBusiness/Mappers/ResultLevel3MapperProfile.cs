using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ResultLevel3MapperProfile : Profile
    {
        public ResultLevel3MapperProfile()
        {
            CreateMap<Result_Level3, Result_Level3DTO>();
            CreateMap<Result_Level3DTO, Result_Level3>();
        }
    }
}
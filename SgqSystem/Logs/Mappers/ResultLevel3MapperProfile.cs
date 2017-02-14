using AutoMapper;
using Dominio;
using static SgqSystem.Controllers.Api.ApontamentosDiariosApiController;

namespace SgqSystem.Mappers
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
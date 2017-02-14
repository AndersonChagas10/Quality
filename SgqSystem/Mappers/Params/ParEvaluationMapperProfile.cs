using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqSystem.Mappers
{
    public class ParEvaluationMapperProfile : Profile
    {
        public ParEvaluationMapperProfile()
        {
            CreateMap<ParEvaluation, ParEvaluationDTO>();
            CreateMap<ParEvaluationDTO, ParEvaluation>();
        }
    }
}
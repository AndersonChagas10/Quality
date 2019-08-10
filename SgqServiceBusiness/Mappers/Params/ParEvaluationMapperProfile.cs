using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
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
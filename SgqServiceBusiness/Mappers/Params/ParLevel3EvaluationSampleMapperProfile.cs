using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParLevel3EvaluationSampleMapperProfile : Profile
    {
        public ParLevel3EvaluationSampleMapperProfile()
        {
            CreateMap<ParLevel3EvaluationSample, ParLevel3EvaluationSampleDTO>();
            CreateMap<ParLevel3EvaluationSampleDTO, ParLevel3EvaluationSample>();
        }
    }
}
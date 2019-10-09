using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqServiceBusiness.Mappers
{
    public class ParGoalMapperProfile : Profile
    {
        public ParGoalMapperProfile()
        {
            CreateMap<ParGoal, ParGoalDTO>();
            CreateMap<ParGoalDTO, ParGoal>();
        }
    }
}
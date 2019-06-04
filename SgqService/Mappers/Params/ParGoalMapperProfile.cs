using AutoMapper;
using Dominio;
using DTO.DTO.Params;

namespace SgqService.Mappers
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
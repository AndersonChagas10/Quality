using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class CorrectiveActionMappingProfile : Profile
    {
        public CorrectiveActionMappingProfile()
        {
            #region CorrectiveAction

            CreateMap<CorrectiveActionDTO, CorrectiveAction>();
            CreateMap<CorrectiveAction, CorrectiveActionDTO>();

            CreateMap<CorrectiveActionLevelsDTO, CorrectiveActionLevels>();
            CreateMap<CorrectiveActionLevels, CorrectiveActionLevelsDTO>();

            #endregion
        }
    }
}
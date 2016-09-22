using AutoMapper;
using Dominio;
using DTO.DTO;

namespace SgqSystem.Mappers
{
    public class CorrectiveActionMapperProfile : Profile
    {
        public CorrectiveActionMapperProfile()
        {
            #region CorrectiveAction

            CreateMap<CorrectiveActionDTO, CorrectiveAction>();
            CreateMap<CorrectiveAction, CorrectiveActionDTO>();

            //CreateMap<CorrectiveActionLevelsDTO, CorrectiveActionLevels>();
            //CreateMap<CorrectiveActionLevels, CorrectiveActionLevelsDTO>();

            #endregion
        }
    }
}
using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;

namespace Dominio.Services
{
    public class CorrectiveActionService : ICorrectiveActionService
    {
        #region Construtor
        private ICorrectiveActionRepository _correctiveActionRepository;

        public CorrectiveActionService(ICorrectiveActionRepository correctiveActionRepository)
        {
            _correctiveActionRepository = correctiveActionRepository;
        }
        #endregion

        public string falhaGeral { get { return "It was not possible retrieve any data."; } }

        public GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto)
        {
            try
            {
                var entitie = Mapper.Map<CorrectiveAction>(dto);
                _correctiveActionRepository.SalvarAcaoCorretiva(entitie);
                return new GenericReturn<CorrectiveActionDTO>(dto);
            }
            catch (Exception e)
            {
                return new GenericReturn<CorrectiveActionDTO>(e, falhaGeral);
            }
        }

        public GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta(CorrectiveActionDTO dto)
        {
            try
            {
                var entitie = Mapper.Map<CorrectiveAction>(dto);
                entitie = _correctiveActionRepository.VerificarAcaoCorretivaIncompleta(entitie);
                return new GenericReturn<CorrectiveActionDTO>(dto);
            }
            catch (Exception e)
            {
                return new GenericReturn<CorrectiveActionDTO>(e, falhaGeral);
            }
        }

    }
}

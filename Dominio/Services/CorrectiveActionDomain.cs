using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;

namespace Dominio.Services
{
    public class CorrectiveActionDomain : ICorrectiveActionDomain
    {
        #region Construtor
        private ICorrectiveActionRepository _correctiveActionRepository;

        public CorrectiveActionDomain(ICorrectiveActionRepository correctiveActionRepository)
        {
            _correctiveActionRepository = correctiveActionRepository;
        }
        #endregion

        public string falhaGeral { get { return "It was not possible retrieve any data."; } }

        public GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto)
        {
            try
            {
                dto.ValidaDataCorrectiveActionDTO();
                var correctiveActionLevels = dto.CorrectiveActionLevels;
                dto.CorrectiveActionLevels = new List<CorrectiveActionLevelsDTO>();

                var entitie = Mapper.Map<CorrectiveAction>(dto);
                entitie = _correctiveActionRepository.SalvarAcaoCorretiva(entitie);


                foreach (var item in correctiveActionLevels)
                {
                    item.ValidaCorrectiveActionLevels();
                    item.CorrectiveActionId = entitie.Id;
                    var entitieLevels = Mapper.Map<CorrectiveActionLevels>(item);
                    entitieLevels = _correctiveActionRepository.SalvarAcaoCorretivaLevels(entitieLevels);
                }

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
                var entitie = _correctiveActionRepository.VerificarAcaoCorretivaIncompleta(Mapper.Map<CorrectiveAction>(dto));
                return new GenericReturn<CorrectiveActionDTO>(Mapper.Map<CorrectiveActionDTO>(entitie));
            }
            catch (Exception e)
            {
                return new GenericReturn<CorrectiveActionDTO>(e, falhaGeral);
            }
        }

    }
}

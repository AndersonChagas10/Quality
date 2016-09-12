using Application.Interface;
using Dominio.Interfaces.Services;
using DTO;
using DTO.DTO;
using DTO.Helpers;
using System.Collections.Generic;
using System;

namespace Application.AppServiceClass
{
    public class CorrectiveActionApp : ICorrectiveActionApp
    {
        #region Contrutor

        private readonly ICorrectiveActionDomain _correctiveActionDomain;

        public CorrectiveActionApp(ICorrectiveActionDomain correctiveActionDomain)
        {
            _correctiveActionDomain = correctiveActionDomain;
        }

        #endregion

        public GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto)
        {
            return _correctiveActionDomain.SalvarAcaoCorretiva(dto);
        }

        public GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta(CorrectiveActionDTO dto)
        {
            return _correctiveActionDomain.VerificarAcaoCorretivaIncompleta(dto);
        }

        public GenericReturn<List<CorrectiveActionDTO>> GetCorrectiveAction(DataCarrierFormulario data)
        {
            return _correctiveActionDomain.GetCorrectiveAction(data);
        }

        public GenericReturn<CorrectiveActionDTO> GetCorrectiveActionById(int id)
        {
            return _correctiveActionDomain.GetCorrectiveActionById(id);
        }
    }
}

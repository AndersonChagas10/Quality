using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;

namespace Dominio.Services
{
    public class ColetaDomain : IColetaDomain
    {
        private IColetaRepository _coletaRepository;

        public string falhaGeral { get { return "It was not possible retrieve any data."; } }
        private string NaoInserido { get { return "It was not possible to insert data."; } }
        private string inseridoOk { get { return "Your data has been successfully saved."; } }

        public ColetaDomain(IColetaRepository coletaRepository)
        {
            _coletaRepository = coletaRepository;
        }

        #region Coleta de Dados.

        public GenericReturn<ColetaDTO> Salvar(ColetaDTO result)
        {
            try
            {
                if (result.IsNull())
                    throw new ExceptionHelper(NaoInserido + " Theres is no data.");

                result.ValidaColeta();
                var objTosave = Mapper.Map<ColetaDTO, Coleta>(result);

                _coletaRepository.ValidaFkResultado(objTosave);

                _coletaRepository.Salvar(objTosave);

                return new GenericReturn<ColetaDTO>(inseridoOk);
            }
            catch (Exception e)
            {
                return new GenericReturn<ColetaDTO>(e, NaoInserido);
            }
        }

        public GenericReturn<ColetaDTO> SalvarLista(List<ColetaDTO> list)
        {
            try
            {
                if (list.IsNull())
                    throw new ExceptionHelper(NaoInserido + " Theres is no data.");

                if (list.Count == 0)
                    throw new ExceptionHelper(NaoInserido + " Theres is no data.");

                foreach (var i in list)
                    i.ValidaColeta();

                var listObjTosave = Mapper.Map<List<ColetaDTO>, List<Coleta>>(list);

                foreach (var i in listObjTosave)
                    _coletaRepository.ValidaFkResultado(i);

                _coletaRepository.SalvarLista(listObjTosave);

                return new GenericReturn<ColetaDTO>(inseridoOk);

            }
            catch (Exception e)
            {
                return new GenericReturn<ColetaDTO>(e, NaoInserido);
            }
        }

        #endregion
    }
}

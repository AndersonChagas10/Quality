using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;

namespace Dominio.Services
{
    public class BetaService :  IBetaService
    {
        private IBetaRepository _betaRepository;

        public string falhaGeral { get { return "It was not possible retrieve any data."; } }
        private string NaoInserido { get { return "It was not possible to insert data."; } }
        private string inseridoOk { get {return "Your data has been successfully saved."; } }

        public BetaService(IBetaRepository relatorioBetaService)
        {
            _betaRepository = relatorioBetaService;
        }

        #region Coleta de Dados.

        public GenericReturn<ResultOldDTO> Salvar(ResultOldDTO result)
        {
            try
            {
                if (result.IsNull())
                    throw new ExceptionHelper(NaoInserido + " Theres is no data.");

                result.ValidaResultOLd();
                var objTosave = Mapper.Map<ResultOldDTO, ResultOld>(result);

                _betaRepository.ValidaFkResultado(objTosave);

                _betaRepository.Salvar(objTosave);

                return new GenericReturn<ResultOldDTO>(inseridoOk);
            }
            catch (Exception e)
            {
                return new GenericReturn<ResultOldDTO>(e, NaoInserido);
            }
        }

        public GenericReturn<ResultOldDTO> SalvarLista(List<ResultOldDTO> list)
        {
            try
            {
                if (list.IsNull())
                    throw new ExceptionHelper(NaoInserido + " Theres is no data.");

                if (list.Count == 0 )
                    throw new ExceptionHelper(NaoInserido + " Theres is no data.");

                foreach (var i in list)
                    i.ValidaResultOLd();

                var listObjTosave = Mapper.Map<List<ResultOldDTO>, List<ResultOld>>(list);

                foreach (var i in listObjTosave)
                    _betaRepository.ValidaFkResultado(i);

                _betaRepository.SalvarLista(listObjTosave);

                return new GenericReturn<ResultOldDTO>(inseridoOk);

            }
            catch (Exception e)
            {
                return new GenericReturn<ResultOldDTO>(e, NaoInserido);
            }
        }

        #endregion

        #region Busca De Dados.

        public GenericReturn<List<ResultOldDTO>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {
            try
            {
                Guard.ForValidFk(indicadorId, "The Level 1 Id is null, " + falhaGeral);

                //VALIDAR AS DATAS AQUI E DEMAIS PARAMETROS

                var retornoRepositorio = _betaRepository.GetNcPorIndicador(indicadorId, dateInit, dateEnd);
                if (retornoRepositorio.Count == 0)
                    throw new ExceptionHelper("No data.");

                var objToReturn = Mapper.Map<List<ResultOld>, List<ResultOldDTO>>(retornoRepositorio);
                return new GenericReturn<List<ResultOldDTO>>(objToReturn);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<ResultOldDTO>>(e, falhaGeral + "(GetNcPorIndicador)");
            }
        }

        public GenericReturn<List<ResultOldDTO>> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd)
        {
            try
            {

                Guard.ForValidFk(indicadorId, "The Level 1 Id is null, " + falhaGeral);

                //VALIDAR AS DATAS AQUI E DEMAIS PARAMETROS

                var retornoRepositorio = _betaRepository.GetNcPorMonitoramento(indicadorId, dateInit, dateEnd);

                var objToReturn = Mapper.Map<List<ResultOld>, List<ResultOldDTO>>(retornoRepositorio);
                return new GenericReturn<List<ResultOldDTO>>(objToReturn);

            }
            catch (Exception e)
            {
                return new GenericReturn<List<ResultOldDTO>>(e, falhaGeral + "(GetNcPorMonitoramento)");
            }
        }

        public GenericReturn<List<ResultOldDTO>> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd)
        {
            try
            {
                Guard.ForValidFk(indicadorId, "The Level 1 Id is null, " + falhaGeral);

                //VALIDAR AS DATAS AQUI E DEMAIS PARAMETROS

                var retornoRepositorio = _betaRepository.GetNcPorTarefa(indicadorId, monitoramentoId, dateInit, dateEnd);

                var objToReturn = Mapper.Map<List<ResultOld>, List<ResultOldDTO>>(retornoRepositorio);
                return new GenericReturn<List<ResultOldDTO>>(objToReturn);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<ResultOldDTO>>(e, falhaGeral + "(GetNcPorTarefa)");
            }
        }

        public GenericReturn<List<ResultOldDTO>> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd)
        {
            try
            {
                Guard.ForValidFk(indicadorId, "The Level 1 Id is null, " + falhaGeral);

                //VALIDAR AS DATAS AQUI E DEMAIS PARAMETROS

                var retornoRepositorio = _betaRepository.GetNcPorMonitoramentoJelsafa(indicadorId, dateInit, dateEnd);

                var objToReturn = Mapper.Map<List<ResultOld>, List<ResultOldDTO>>(retornoRepositorio);
                return new GenericReturn<List<ResultOldDTO>>(objToReturn);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<ResultOldDTO>>(e, falhaGeral +"(GetNcPorMonitoramentoJelsafa)");
            }
        }

        #endregion

    }
}

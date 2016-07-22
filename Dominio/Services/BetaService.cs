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

        public GenericReturn<ColetaDTO> Salvar(ColetaDTO result)
        {
            try
            {
                if (result.IsNull())
                    throw new ExceptionHelper(NaoInserido + " Theres is no data.");

                result.ValidaColeta();
                var objTosave = Mapper.Map<ColetaDTO, Coleta>(result);

                _betaRepository.ValidaFkResultado(objTosave);

                _betaRepository.Salvar(objTosave);

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

                if (list.Count == 0 )
                    throw new ExceptionHelper(NaoInserido + " Theres is no data.");

                foreach (var i in list)
                    i.ValidaColeta();

                var listObjTosave = Mapper.Map<List<ColetaDTO>, List<Coleta>>(list);

                foreach (var i in listObjTosave)
                    _betaRepository.ValidaFkResultado(i);

                _betaRepository.SalvarLista(listObjTosave);

                return new GenericReturn<ColetaDTO>(inseridoOk);

            }
            catch (Exception e)
            {
                return new GenericReturn<ColetaDTO>(e, NaoInserido);
            }
        }

        #endregion

        #region Busca De Dados.

        public GenericReturn<List<ColetaDTO>> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {
            try
            {
                Guard.ForValidFk(indicadorId, "The Level 1 Id is null, " + falhaGeral);

                //VALIDAR AS DATAS AQUI E DEMAIS PARAMETROS

                var retornoRepositorio = _betaRepository.GetNcPorIndicador(indicadorId, dateInit, dateEnd);
                if (retornoRepositorio.Count == 0)
                    throw new ExceptionHelper("No data.");

                var objToReturn = Mapper.Map<List<Coleta>, List<ColetaDTO>>(retornoRepositorio);
                return new GenericReturn<List<ColetaDTO>>(objToReturn);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<ColetaDTO>>(e, falhaGeral + "(GetNcPorIndicador)");
            }
        }

        public GenericReturn<List<ColetaDTO>> GetNcPorLevel2(int indicadorId, string dateInit, string dateEnd)
        {
            try
            {

                Guard.ForValidFk(indicadorId, "The Level 1 Id is null, " + falhaGeral);

                //VALIDAR AS DATAS AQUI E DEMAIS PARAMETROS

                var retornoRepositorio = _betaRepository.GetNcPorLevel2(indicadorId, dateInit, dateEnd);

                var objToReturn = Mapper.Map<List<Coleta>, List<ColetaDTO>>(retornoRepositorio);
                return new GenericReturn<List<ColetaDTO>>(objToReturn);

            }
            catch (Exception e)
            {
                return new GenericReturn<List<ColetaDTO>>(e, falhaGeral + "(GetNcPorLevel2)");
            }
        }

        public GenericReturn<List<ColetaDTO>> GetNcPorLevel3(int indicadorId, int Level2Id, string dateInit, string dateEnd)
        {
            try
            {
                Guard.ForValidFk(indicadorId, "The Level 1 Id is null, " + falhaGeral);

                //VALIDAR AS DATAS AQUI E DEMAIS PARAMETROS

                var retornoRepositorio = _betaRepository.GetNcPorLevel3(indicadorId, Level2Id, dateInit, dateEnd);

                var objToReturn = Mapper.Map<List<Coleta>, List<ColetaDTO>>(retornoRepositorio);
                return new GenericReturn<List<ColetaDTO>>(objToReturn);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<ColetaDTO>>(e, falhaGeral + "(GetNcPorLevel3)");
            }
        }

        public GenericReturn<List<ColetaDTO>> GetNcPorLevel2Jelsafa(int indicadorId, string dateInit, string dateEnd)
        {
            try
            {
                Guard.ForValidFk(indicadorId, "The Level 1 Id is null, " + falhaGeral);

                //VALIDAR AS DATAS AQUI E DEMAIS PARAMETROS

                var retornoRepositorio = _betaRepository.GetNcPorLevel2Jelsafa(indicadorId, dateInit, dateEnd);

                var objToReturn = Mapper.Map<List<Coleta>, List<ColetaDTO>>(retornoRepositorio);
                return new GenericReturn<List<ColetaDTO>>(objToReturn);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<ColetaDTO>>(e, falhaGeral +"(GetNcPorLevel2Jelsafa)");
            }
        }

        #endregion

    }
}

﻿using Data.PlanoDeAcao.Interfaces;
using Data.PlanoDeAcao.Repositorio;
using Dominio;
using DTO.PlanoDeAcao;
using Helper;
using SgqServiceBusiness.Controllers.RH;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.PlanoDeAcao
{
    public class EvidenciaConcluidaService : Interfaces.IEvidenciaConcluidaService
    {
        private readonly IEvidenciaConcluidaRepository _evidenciaConcluidaRepository;
        private readonly IAcaoRepository _acaoRepository;

        public EvidenciaConcluidaService(IEvidenciaConcluidaRepository evidenciaConcluidaRepository, IAcaoRepository acaoRepository)
        {
            _evidenciaConcluidaRepository = evidenciaConcluidaRepository;
            _acaoRepository = acaoRepository;
        }

        public List<ImagemDaEvidenciaViewModel> ObterFotosEvidenciaConcluida(int id)
        {
            var lista = new AcaoFormViewModel();

            var listaFotos = new List<ImagemDaEvidenciaViewModel>();

            lista.ListaEvidenciaConcluida = _evidenciaConcluidaRepository.BuscarListaEvidenciasConcluidas(id);

            foreach (var item in lista.ListaEvidenciaConcluida)
            {
                var foto = new ImagemDaEvidenciaViewModel();

                string url = item.Path;

                byte[] bytes;
                //Verificar se no web.config a credencial do servidor de fotos
                Exception exception = null;

                bytes = FileHelper.DownloadPhoto(url
                , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialUserServerPhoto
                , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialPassServerPhoto
                , out exception);

                if (exception != null)
                    throw new Exception("Error: " + exception.ToClient());

                foto.Byte = bytes;
                foto.Path = item.Path;
                listaFotos.Add(foto);
            }
            return listaFotos;
        }

        public void RetornarListaDeEvidenciasConcluidas(AcaoInputModel objAcao)
        {
            var listaEvidenciasConcluidasDB = _evidenciaConcluidaRepository.BuscarListaEvidenciasConcluidas(objAcao.Id);

            var listaEvidenciasPathsEditadas = new List<EvidenciaViewModel>();
            if (objAcao.ListaEvidenciaConcluida != null)
            {
                listaEvidenciasPathsEditadas = objAcao.ListaEvidenciaConcluida.ToList();
            }

            var listaInserir = listaEvidenciasPathsEditadas.Where(x => !listaEvidenciasConcluidasDB.Select(y => y.Path).ToList().Contains(x.Path)).ToList();

            var listaDeletar = listaEvidenciasConcluidasDB.Where(x => !listaEvidenciasPathsEditadas.Select(y => y.Path).Contains(x.Path)).ToList();

            if (listaInserir.Count > 0)
                VincularEvidenciasAAcaoConcluida(objAcao, listaInserir);

            if (listaDeletar.Count > 0)
                _evidenciaConcluidaRepository.InativarEvidenciasDaAcaoConcluida(listaDeletar);
        }

        public void VincularEvidenciasAAcaoConcluida(AcaoInputModel objAcao, List<EvidenciaViewModel> listaInserir)
        {
            AppColetaBusiness appColetaBusiness = new AppColetaBusiness();

            var objAcaoDB = _acaoRepository.ObterAcaoPorId(objAcao.Id);

            foreach (var evidenciaAcaoConcluida in listaInserir)
            {
                var filePath = appColetaBusiness.SaveFileEvidenciaAcaoConcluida(objAcaoDB.ParLevel1_Id, objAcaoDB.ParLevel2_Id, objAcaoDB.ParLevel3_Id, evidenciaAcaoConcluida.Base64);
                appColetaBusiness.SaveEvidenciaAcaoConcluida(new EvidenciaAcaoConcluida() { Acao_Id = objAcao.Id, Path = filePath });
            }
        }
    }
}

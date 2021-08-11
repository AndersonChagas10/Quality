using Conformity.Application.Core.Log;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using Conformity.Domain.Core.Entities.Global;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.CrossCutting;
using Conformity.Infra.Data.Core.Repository.PlanoDeAcao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Conformity.Application.Core.PlanoDeAcao
{
    public class EvidenciaConcluidaService : BaseServiceWithLog<EvidenciaConcluida>
    {

        private readonly EvidenciaConcluidaRepository _evidenciaConcluidaRepository;
        private readonly AcaoRepository _acaoRepository;
        private readonly LogErrorService _logErrorService;

        public EvidenciaConcluidaService(IPlanoDeAcaoRepositoryNoLazyLoad<EvidenciaConcluida> repository
            , EntityTrackService historicoAlteracaoService
            , EvidenciaConcluidaRepository evidenciaConcluidaRepository
            , LogErrorService logErrorService
            , AcaoRepository acaoRepository)
            : base(repository
                  , historicoAlteracaoService)
        {
            _evidenciaConcluidaRepository = evidenciaConcluidaRepository;
            _acaoRepository = acaoRepository;
            _logErrorService = logErrorService;
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
                , DicionarioEstatico.DicionarioEstaticoHelpers.credentialUserServerPhoto
                , DicionarioEstatico.DicionarioEstaticoHelpers.credentialPassServerPhoto
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
            foreach (var evidenciaAcaoConcluida in listaInserir)
            {
                var filePath = SaveFileEvidenciaAcaoConcluida(objAcao.Id, objAcao.ParCompany_Id, evidenciaAcaoConcluida.Base64);
                _evidenciaConcluidaRepository.SalvarEvidenciaAcaoConcluida(new EvidenciaConcluida() { Acao_Id = objAcao.Id, Path = filePath });
            }
        }

        
        private string SaveFileEvidenciaAcaoConcluida(int acaoId, int parCompany_Id, string fileBase64)
        {
            var basePath = DicionarioEstatico.DicionarioEstaticoHelpers.StorageRoot ?? "~";
            if (basePath.Equals("~"))
            {
                basePath = @AppDomain.CurrentDomain.BaseDirectory;
            }

            basePath = basePath + "\\Acao";
            string fileName = acaoId + parCompany_Id + DateTime.Now.GetHashCode() + new Random().Next(1000, 9999) + ".png";

            Exception exception;

            FileHelper.SavePhoto(fileBase64, basePath, fileName
                      , DicionarioEstatico.DicionarioEstaticoHelpers.credentialUserServerPhoto
                      , DicionarioEstatico.DicionarioEstaticoHelpers.credentialPassServerPhoto
                      , DicionarioEstatico.DicionarioEstaticoHelpers.StorageRoot, out exception);

            if (exception != null)
                _logErrorService.Register(exception);

            var path = Path.Combine(basePath, fileName);

            return path;
        }

        public List<string> ObterEvidenciaConcluidaEmFormatoBase64(int acaoId)
        {
            var listaDeEvidencias = ObterFotosEvidenciaConcluida(acaoId);
            List<string> ListaDeEvidenciasEmFormatoString = new List<string>();

            foreach (var item in listaDeEvidencias)
            {
                ListaDeEvidenciasEmFormatoString.Add(Convert.ToBase64String(item.Byte));
            }

            return ListaDeEvidenciasEmFormatoString;
        }
    }
}

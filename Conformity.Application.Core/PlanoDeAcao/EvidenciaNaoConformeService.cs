using Conformity.Application.Core.Log;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities.Global;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.CrossCutting;
using Conformity.Infra.Data.Core.Repository.PlanoDeAcao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Conformity.Application.Core.PlanoDeAcao
{
    public class EvidenciaNaoConformeService : BaseServiceWithLog<EvidenciaNaoConforme>
    {

        private readonly EvidenciaNaoConformeRepository _evidenciaNaoConformeRepository;
        private readonly AcaoRepository _acaoRepository;
        private readonly LogErrorService _logErrorService;

        public EvidenciaNaoConformeService(IPlanoDeAcaoRepositoryNoLazyLoad<EvidenciaNaoConforme> repository
            , LogErrorService logErrorService
            , EntityTrackService historicoAlteracaoService
            , EvidenciaNaoConformeRepository evidenciaNaoConformeRepository
            , AcaoRepository acaoRepository)
            : base(repository
                  , historicoAlteracaoService)
        {
            _evidenciaNaoConformeRepository = evidenciaNaoConformeRepository;
            _acaoRepository = acaoRepository;
            _logErrorService = logErrorService;
        }
        public List<ImagemDaEvidenciaViewModel> ObterFotosEvidencia(int id)
        {
            var lista = new AcaoFormViewModel();

            var listaFotos = new List<ImagemDaEvidenciaViewModel>();

            ////download das imagens
            lista.ListaEvidencia = _evidenciaNaoConformeRepository.BuscarListaEvidencias(id);

            foreach (var item in lista.ListaEvidencia)
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

        public void RetornarListaDeEvidencias(AcaoInputModel objAcao)
        {
            var listaEvidenciasDB = _evidenciaNaoConformeRepository.RetornarEvidenciasDaAcao(objAcao.Id);
            var listaEvidenciasPathsEditadas = new List<EvidenciaViewModel>();

            if (objAcao.ListaEvidencia != null)
            {
                listaEvidenciasPathsEditadas = objAcao.ListaEvidencia.ToList();
            }

            var listaInserir = listaEvidenciasPathsEditadas.Where(x => !listaEvidenciasDB.Select(y => y.Path).ToList().Contains(x.Path)).ToList();

            var listaDeletar = listaEvidenciasDB.Where(x => !listaEvidenciasPathsEditadas.Select(y => y.Path).Contains(x.Path)).ToList();

            if (listaInserir.Count > 0)
                VincularEvidenciasAAcao(objAcao, listaInserir);

            if (listaDeletar.Count > 0)
                _evidenciaNaoConformeRepository.InativarEvidencias(listaDeletar);
        }

        public void VincularEvidenciasAAcao(AcaoInputModel objAcao, List<EvidenciaViewModel> listaInserir)
        {
            foreach (var evidenciaNaoConformidade in listaInserir)
            {
                var filePath = SaveFileEvidenciaNaoConformidade(objAcao.Id, objAcao.ParCompany_Id, evidenciaNaoConformidade.Base64);
                _evidenciaNaoConformeRepository.SaveEvidenciaNaoConformidade(new EvidenciaNaoConforme() { Acao_Id = objAcao.Id, Path = filePath });
            }
        }

        private string SaveFileEvidenciaNaoConformidade(int acaoId, int parCompany_Id, string fileBase64)
        {
            var basePath = DicionarioEstatico.DicionarioEstaticoHelpers.StorageRoot ?? "~";

            if (basePath.Equals("~"))
            {
                basePath = @AppDomain.CurrentDomain.BaseDirectory;
            }

            basePath = basePath + "\\Acao";
            string fileName = acaoId + parCompany_Id + new Random().Next(1000, 9999) + ".png";

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
    }
}

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
    public class EvidenciaNaoConformeService : IEvidenciaNaoConformeService
    {
        private readonly IEvidenciaNaoConformeRepository _evidenciaNaoConformeRepository;
        public readonly IAcaoRepository _acaoRepository;
        public EvidenciaNaoConformeService(IEvidenciaNaoConformeRepository evidenciaNaoConforme, IAcaoRepository acaoRepository)
        {
            _evidenciaNaoConformeRepository = evidenciaNaoConforme;
            _acaoRepository = acaoRepository;
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
            AppColetaBusiness appColetaBusiness = new AppColetaBusiness();

            var objAcaoDB = _acaoRepository.ObterAcaoPorId(objAcao.Id);

            foreach (var evidenciaNaoConformidade in listaInserir)
            {
                var filePath = appColetaBusiness.SaveFileEvidenciaNaoConformidade(objAcaoDB.ParLevel1_Id, objAcaoDB.ParLevel2_Id, objAcaoDB.ParLevel3_Id, evidenciaNaoConformidade.Base64);
                appColetaBusiness.SaveEvidenciaNaoConformidade(new EvidenciaNaoConformidade() { Acao_Id = objAcao.Id, Path = filePath });
            }
        }
    }
}

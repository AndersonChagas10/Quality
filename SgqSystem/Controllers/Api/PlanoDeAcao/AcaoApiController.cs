using Data.PlanoDeAcao.Repositorio;
using Dominio;
using Dominio.AcaoRH;
using Dominio.AcaoRH.Email;
using DTO;
using DTO.PlanoDeAcao;
using Helper;
using SgqServiceBusiness.Controllers.RH;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/AcaoApi")]
    public class AcaoApiController : BaseApiController
    {
        public readonly IAcaoRepository _acaoRepository;
        public AcaoApiController(IAcaoRepository acaoRepository)
        {
            _acaoRepository = acaoRepository;
        }

        [Route("GetAcaoByFilter")]
        [HttpPost]
        public IEnumerable<AcaoViewModel> GetAcaoByFilter([FromBody] DataCarrierFormularioNew form)
        {
            return _acaoRepository.ObterAcaoPorFiltro(form);
        }

        [Route("GetByIdStatus/{status}")]
        [HttpGet]
        public IEnumerable<AcaoViewModel> GetByIdStatus(string status)
        {
            return _acaoRepository.ObterStatusPorId(status);
        }

        [Route("Post")]
        [HttpPost]
        public AcaoViewModel Post([FromBody] AcaoInputModel objAcao)
        {
            try
            {

                //salva os campos comuns da ação
                _acaoRepository.AtualizarValoresDaAcao(objAcao);

                //salva/deleta a listagem de usuario no campo notificar
                AtualizarUsuariosASeremNotificadosDaAcao(objAcao);

                //salva/deleta a listagem de imagens de evidencias
                RetornarListaDeEvidencias(objAcao);

                RetornarListaDeEvidenciasConcluidas(objAcao);

                if (objAcao.Responsavel != null)
                {
                    PrepararEEnviarEmail(objAcao);
                }

            }
            catch (Exception e)
            {
                return null;
            }

            return new AcaoViewModel() { Id = objAcao.Id };
        }

        [Route("Post/Acompanhamento/{id}")]
        [HttpPost]
        public AcaoViewModel Post([FromUri] int id, [FromBody] AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            try
            {
                var usuarioLogado = base.GetUsuarioLogado();

                var listaNotificar = objAcompanhamentoAcao.ListaNotificar
                    .Select(x =>
                        new AcompanhamentoAcaoXNotificar()
                        {
                            UserSgq_Id = x.Id
                        }
                    ).ToList();

                var acompanhamento = new AcompanhamentoAcao()
                {
                    ListaNotificar = listaNotificar,
                    Observacao = objAcompanhamentoAcao.Observacao,
                    Status = objAcompanhamentoAcao.Status,
                    Acao_Id = id,
                    UserSgq_Id = usuarioLogado.Id
                };

                _acaoRepository.SalvarAcompanhamentoAcao(acompanhamento);

            }
            catch (Exception e)
            {
                return null;
            }

            return null;
        }

        [Route("GetById/{id}")]
        [HttpGet]
        public AcaoFormViewModel GetById(int id)
        {
            return _acaoRepository.ObterAcaoComVinculosPorId(id);
        }

        [Route("GetFotosEvidenciaConcluida/{id}")]
        [HttpGet]
        public List<ImagemDaEvidenciaViewModel> GetFotosEvidenciaConcluida(int id)
        {
            var lista = new AcaoFormViewModel();

            var listaFotos = new List<ImagemDaEvidenciaViewModel>();

            lista.ListaEvidenciaConcluida = _acaoRepository.BuscarListaEvidenciasConcluidas(id);

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

        [Route("GetFotosEvidencia/{id}")]
        [HttpGet]
        public List<ImagemDaEvidenciaViewModel> GetFotosEvidencia(int id)
        {
            var lista = new AcaoFormViewModel();

            var listaFotos = new List<ImagemDaEvidenciaViewModel>();

            ////download das imagens
            lista.ListaEvidencia = _acaoRepository.BuscarListaEvidencias(id);

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

        #region AcaoService
        private void PrepararEEnviarEmail(AcaoInputModel acao)
        {
            //1 Pendente - nao envia email
            //2 Em andamento - cenario 1 e 2
            //3 Concluída - cenario 3 e 4
            //4 Atrasada  - cenario 5 e 6
            //5 Cancelada - cenario 7 e 8

            if (int.Parse(acao.Status) == 2)
            {
                var acaoCompleta = new AcaoBusiness().GetBy(acao.Id);

                var emailResponsavel = new MontaEmail(new EmailCreateAcaoResponsavel(acaoCompleta));
                EmailAcaoService.Send(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoNotificados(acaoCompleta));
                EmailAcaoService.Send(emailNotificados);
            }

            if (int.Parse(acao.Status) == 3)
            {
                var acaoCompleta = new AcaoBusiness().GetBy(acao.Id);

                var emailResponsavel = new MontaEmail(new EmailCreateAcaoVerEAgirResponsavel(acaoCompleta));
                EmailAcaoService.Send(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoVerEAgirNotificados(acaoCompleta));
                EmailAcaoService.Send(emailNotificados);

            }
        }

        private void RetornarListaDeEvidencias(AcaoInputModel objAcao)
        {
            var listaEvidenciasDB = _acaoRepository.RetornarEvidenciasDaAcao(objAcao.Id);
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
                _acaoRepository.InativarEvidencias(listaDeletar);
        }

        private void RetornarListaDeEvidenciasConcluidas(AcaoInputModel objAcao)
        {
            var listaEvidenciasConcluidasDB = _acaoRepository.BuscarListaEvidenciasConcluidas(objAcao.Id);

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
                _acaoRepository.InativarEvidenciasDaAcaoConcluida(listaDeletar);
        }

        private void VincularEvidenciasAAcao(AcaoInputModel objAcao, List<EvidenciaViewModel> listaInserir)
        {
            AppColetaBusiness appColetaBusiness = new AppColetaBusiness();

            var objAcaoDB = _acaoRepository.ObterAcaoPorId(objAcao.Id);

            foreach (var evidenciaNaoConformidade in listaInserir)
            {
                var filePath = appColetaBusiness.SaveFileEvidenciaNaoConformidade(objAcaoDB.ParLevel1_Id, objAcaoDB.ParLevel2_Id, objAcaoDB.ParLevel3_Id, evidenciaNaoConformidade.Base64);
                appColetaBusiness.SaveEvidenciaNaoConformidade(new EvidenciaNaoConformidade() { Acao_Id = objAcao.Id, Path = filePath });
            }
        }

        private void VincularEvidenciasAAcaoConcluida(AcaoInputModel objAcao, List<EvidenciaViewModel> listaInserir)
        {
            AppColetaBusiness appColetaBusiness = new AppColetaBusiness();

            var objAcaoDB = _acaoRepository.ObterAcaoPorId(objAcao.Id);

            foreach (var evidenciaAcaoConcluida in listaInserir)
            {
                var filePath = appColetaBusiness.SaveFileEvidenciaAcaoConcluida(objAcaoDB.ParLevel1_Id, objAcaoDB.ParLevel2_Id, objAcaoDB.ParLevel3_Id, evidenciaAcaoConcluida.Base64);
                appColetaBusiness.SaveEvidenciaAcaoConcluida(new EvidenciaAcaoConcluida() { Acao_Id = objAcao.Id, Path = filePath });
            }
        }

        private void AtualizarUsuariosASeremNotificadosDaAcao(AcaoInputModel objAcao)
        {
            var listaUsuarioNotificados = _acaoRepository.RetornarUsuariosASeremNotificadosDaAcao(objAcao);

            var listaIdsUsuarioEditados = objAcao.ListaNotificarAcao.Select(x => x.Id).ToList();

            var listaInserir = listaIdsUsuarioEditados.Where(x => !listaUsuarioNotificados.Select(y => y.UserSgq_Id).ToList().Contains(x)).ToList();

            var listaDeletar = listaUsuarioNotificados.Select(y => y.UserSgq_Id).ToList().Where(x => !listaIdsUsuarioEditados.Contains(x)).ToList();

            if (listaInserir.Count > 0)
                _acaoRepository.VincularUsuariosASeremNotificadosAAcao(objAcao, listaInserir);

            if (listaDeletar.Count > 0)
                _acaoRepository.InativarUsuariosASeremNotificadosAAcao(objAcao, listaDeletar);

        }

        #endregion
    }
}

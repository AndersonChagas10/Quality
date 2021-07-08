using Data.PlanoDeAcao.Repositorio;
using Dominio;
using DTO;
using DTO.PlanoDeAcao;
using Services.PlanoDeAcao;
using SgqServiceBusiness.Controllers.RH;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/AcaoApi")]
    public class AcaoApiController : BaseApiController
    {
        public readonly IAcaoRepository _acaoRepository;
        public readonly IAcaoService _acaoService;
        public readonly IEvidenciaNaoConformeService _evidenciaNaoConformeService;
        private readonly IEvidenciaConcluidaService _evidenciaConcluidaService;
        public AcaoApiController(IAcaoRepository acaoRepository, IAcaoService acaoService,
            IEvidenciaNaoConformeService evidenciaNaoConformeService,
            IEvidenciaConcluidaService evidenciaConcluidaService)
        {
            _acaoRepository = acaoRepository;
            _acaoService = acaoService;
            _evidenciaNaoConformeService = evidenciaNaoConformeService;
            _evidenciaConcluidaService = evidenciaConcluidaService;
        }

        [Route("GetAcaoByFilter")]
        [HttpPost]
        public IEnumerable<AcaoViewModel> GetAcaoByFilter([FromBody] DataCarrierFormularioNew form)
        {
            var usuarioLogado = base.GetUsuarioLogado();
            return _acaoService.ObterAcaoPorFiltro(form, usuarioLogado);
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
                _evidenciaNaoConformeService.RetornarListaDeEvidencias(objAcao);

                _evidenciaConcluidaService.RetornarListaDeEvidenciasConcluidas(objAcao);

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

        [Route("SalvarAcao")]
        [HttpPost]
        public int SalvarAcao([FromBody] Acao objAcao)
        {
            int Id = 0;
            try
            {
                //AppColetaBusiness appColetaBusiness = new AppColetaBusiness();
                //appColetaBusiness.SalvarAcao(objAcao);

                // salva os campos comuns da ação
                Id =  _acaoRepository.SalvarAcao(objAcao);
                 
                //salva/deleta a listagem de usuario no campo notificar
                //AtualizarUsuariosASeremNotificadosDaAcao(objAcao);

                ////salva/deleta a listagem de imagens de evidencias
                //_evidenciaNaoConformeService.RetornarListaDeEvidencias(objAcao);

                //_evidenciaConcluidaService.RetornarListaDeEvidenciasConcluidas(objAcao);

                //if (objAcao.Responsavel != null)
                //{
                //    PrepararEEnviarEmail(objAcao);
                //}

            }
            catch (Exception e)
            {
                //
            }

            return  Id ;
        }
 
        [Route("GetById/{id}")]
        [HttpGet]
        public AcaoFormViewModel GetById(int id)
        {
            var usuarioLogado = base.GetUsuarioLogado();
            return _acaoRepository.ObterAcaoComVinculosPorId(id, usuarioLogado);
        }

        private void PrepararEEnviarEmail(AcaoInputModel acao)
        {
            _acaoService.EnviarEmail(acao);
        }

        private void AtualizarUsuariosASeremNotificadosDaAcao(AcaoInputModel objAcao)
        {
            _acaoService.AtualizarUsuarios(objAcao);

        }
    }
}

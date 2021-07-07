using Conformity.Application.Core.PlanoDeAcao;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.DTOs.Filtros;
using Conformity.Infra.CrossCutting;
using Dominio;
using DTO;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/AcaoApi")]
    public class AcaoApiController : BaseApiController
    {
        public readonly AcaoService _acaoService;
        public readonly EvidenciaNaoConformeService _evidenciaNaoConformeService;
        private readonly EvidenciaConcluidaService _evidenciaConcluidaService;
        public AcaoApiController(AcaoService acaoService
            , ApplicationConfig applicationConfig
            , EvidenciaNaoConformeService evidenciaNaoConformeService
            , EvidenciaConcluidaService evidenciaConcluidaService) : base()
        {
            _acaoService = acaoService;
            _evidenciaNaoConformeService = evidenciaNaoConformeService;
            _evidenciaConcluidaService = evidenciaConcluidaService;
        }

        [Route("GetAcaoByFilter")]
        [HttpPost]
        public IEnumerable<AcaoViewModel> GetAcaoByFilter([FromBody] FiltroListagemDeAcaoDoWorkflow form)
        {
            return _acaoService.ObterAcaoPorFiltro(form);
        }

        [Route("GetByIdStatus/{status}")]
        [HttpGet]
        public IEnumerable<AcaoViewModel> GetByIdStatus(string status)
        {
            return null;
            // return _acaoService.(status);
        }

        [Route("Post")]
        [HttpPost]
        public AcaoViewModel Post([FromBody] AcaoInputModel objAcao)
        {
            try
            {
                //salva os campos comuns da ação
                _acaoService.AtualizarValoresDaAcao(objAcao);

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


        [Route("GetById/{id}")]
        [HttpGet]
        public AcaoFormViewModel GetById(int id)
        {
            var usuarioLogado = base.GetUsuarioLogado();
            return _acaoService.ObterAcaoComVinculosPorId(id);
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

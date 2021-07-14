using Conformity.Application.Core.PlanoDeAcao;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.DTOs.Filtros;
using Conformity.Infra.CrossCutting;
using Dominio;
using DTO;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using static Conformity.Domain.Core.Enums.PlanoDeAcao.Enums;
using ParCompany = Conformity.Domain.Core.Entities.PlanoDeAcao.ParCompany;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/AcaoApi")]
    public class AcaoApiController : BaseAuthenticatedApiController
    {
        public readonly AcaoService _acaoService;
        public readonly EvidenciaNaoConformeService _evidenciaNaoConformeService;
        private readonly EvidenciaConcluidaService _evidenciaConcluidaService;
        public AcaoApiController(AcaoService acaoService
            , ApplicationConfig applicationConfig
            , EvidenciaNaoConformeService evidenciaNaoConformeService
            , EvidenciaConcluidaService evidenciaConcluidaService) : base(applicationConfig)
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

        [Route("GetUnityByCurrentUser")]
        [HttpPost]
        public IEnumerable<ParCompany> GetUnityByCurrentUser(string search)
        {
            return _acaoService.GetUnityByCurrentUser(search);
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

                if (objAcao.Status != EAcaoStatus.Pendente)
                {
                    PrepararEEnviarEmail(objAcao.Id);
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
        public int SalvarAcao([FromBody] Conformity.Domain.Core.Entities.PlanoDeAcao.Acao objAcao)
        {
            int Id = _acaoService.SalvarAcao(objAcao);
            return Id;
        }
 
        [Route("GetById/{id}")]
        [HttpGet]
        public AcaoFormViewModel GetById(int id)
        {
            return _acaoService.ObterAcaoComVinculosPorId(id);
        }

        private void PrepararEEnviarEmail(int acaoId)
        {
            _acaoService.EnviarEmail(acaoId);
        }

        private void AtualizarUsuariosASeremNotificadosDaAcao(AcaoInputModel objAcao)
        {
            _acaoService.AtualizarUsuarios(objAcao);

        }
    }
}

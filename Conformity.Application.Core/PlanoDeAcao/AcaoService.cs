﻿using Conformity.Application.Core.Log;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.DTOs.Filtros;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using Conformity.Domain.Core.Entities.Global;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core.Repository.PlanoDeAcao;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Conformity.Domain.Core.Enums.PlanoDeAcao.Enums;
using Conformity.Domain.Core.Enums.PlanoDeAcao;
using Conformity.Domain.Core.DTOs.PlanoDeAcao;

namespace Conformity.Application.Core.PlanoDeAcao
{
    public class AcaoService : BaseServiceWithLog<Acao>
    {
        private readonly AcaoRepository _acaoRepository;
        private readonly EvidenciaConcluidaService _evidenciaConcluidaService;
        private readonly EvidenciaNaoConformeService _evidenciaNaoConfomeService;

        public AcaoService(IPlanoDeAcaoRepositoryNoLazyLoad<Acao> repository
            , EntityTrackService historicoAlteracaoService,
            EvidenciaConcluidaService evidenciaConcluidaService,
            EvidenciaNaoConformeService evidenciaNaoConfomeService,
            AcaoRepository acaoRepository)
            : base(repository
                  , historicoAlteracaoService)
        {
            _acaoRepository = acaoRepository;
            _evidenciaConcluidaService = evidenciaConcluidaService;
            _evidenciaNaoConfomeService = evidenciaNaoConfomeService;
        }
        public IEnumerable<AcaoViewModel> ObterAcaoPorFiltro(FiltroListagemDeAcaoDoWorkflow form)
        {
            return _acaoRepository.ObterAcao(form);
        }

        public void EnviarEmail(int? acompanhamentoId, int acaoId)
        {
            //1 Pendente - nao envia email
            //2 Em andamento - cenario 1 e 2
            //3 Concluída - cenario 3 e 4
            //4 Atrasada  - cenario 5 e 6
            //5 Cancelada - cenario 7 e 8

            var acaoCompleta = _acaoRepository.GetById(acaoId);
            acaoCompleta.NotificarUsers = _acaoRepository.ObterNotificaveisDaAcao(acaoId);
            acaoCompleta.EvidenciaAcaoConcluida = _evidenciaConcluidaService.ObterEvidenciaConcluidaEmFormatoBase64(acaoId);
            acaoCompleta.EvidenciaNaoConformidade = _evidenciaNaoConfomeService.ObterEvidenciaNaoConformeEmFormatoBase64(acaoId);

            if (acompanhamentoId.HasValue)
            {
                acaoCompleta.NotificarUsers = _acaoRepository.ObterNotificaveisDoAcompanhamento(acompanhamentoId.Value);
            }            

            if (acaoCompleta.Status == EAcaoStatus.Em_Andamento)
            {
                var emailResponsavel = new MontaEmail(new EmailCreateAcaoResponsavel(acaoCompleta));
                EnviarEmailAcao(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoNotificados(acaoCompleta));
                EnviarEmailAcao(emailNotificados);
            }

            else if (acaoCompleta.Status == EAcaoStatus.Concluída)
            {     
                var emailResponsavel = new MontaEmail(new EmailCreateAcaoVerEAgirResponsavel(acaoCompleta));
                EnviarEmailAcao(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoVerEAgirNotificados(acaoCompleta));
                EnviarEmailAcao(emailNotificados);

            }

            else if (acaoCompleta.Status == EAcaoStatus.Atrasada)
            {
                var emailResponsavel = new MontaEmail(new EmailAcaoVencidaResponsavel(acaoCompleta));
                EnviarEmailAcao(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailAcaoVencidaNotificados(acaoCompleta));
                EnviarEmailAcao(emailNotificados);
            }

            else if (acaoCompleta.Status == EAcaoStatus.Cancelada)
            {
                var emailResponsavel = new MontaEmail(new EmailAcaoStatusCanceladoResponsavel(acaoCompleta));
                EnviarEmailAcao(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailAcaoStatusCanceladoNotificados(acaoCompleta));
                EnviarEmailAcao(emailNotificados);
            }
        }

        public int SalvarAcao(Acao objAcao)
        {
            int acaoId = _acaoRepository.SalvarAcao(objAcao);

            SalvarCodigoDaAcao(objAcao);

            return acaoId;
        }    
        
        private void SalvarCodigoDaAcao(Acao objAcao)
        {
            AcaoXProximoCodigoViewModel value = _acaoRepository.GerarCodigoDaAcao(objAcao);

            AcaoXAttributes acaoXAttributes = new AcaoXAttributes
            {
                Acao_Id = objAcao.Id,
                FieldName = EAcaoXAttributes.CodigoDaAcao,
                Value = value.ProximoCodigo
            };

            _acaoRepository.SalvarCodigoDaAcao(acaoXAttributes);
        }

        public List<ParCompany> GetUnityByCurrentUser(string search)
        {
            return _acaoRepository.GetUnityByCurrentUser(search);
        }

        public void AtualizarUsuarios(AcaoInputModel objAcao)
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

        public static void EnviarEmailAcao(MontaEmail email)
        {
            string emailFrom = (DicionarioEstatico.DicionarioEstaticoHelpers.emailFrom as string);
            string emailPass = (DicionarioEstatico.DicionarioEstaticoHelpers.emailPass as string);
            string emailSmtp = (DicionarioEstatico.DicionarioEstaticoHelpers.emailSmtp as string);
            int emailPort = int.Parse(DicionarioEstatico.DicionarioEstaticoHelpers.emailPort as string);
            bool emailSSL = bool.Parse(DicionarioEstatico.DicionarioEstaticoHelpers.emailSSL as string);
            string systemName = (DicionarioEstatico.DicionarioEstaticoHelpers.systemName as string);
            string toAddress = string.Join(",", email.Email.To);
            Task.Run(() =>
            Conformity.Infra.CrossCutting.MailHelper.SendMail(
                emailFrom,
                emailPass,
                emailSmtp,
                emailPort,
                emailSSL,
                toAddress,
                email.Email.Subject,
                email.Email.Body,
                systemName)
            );
        }

        public AcaoFormViewModel ObterAcaoComVinculosPorId(int id)
        {
            return _acaoRepository.ObterAcaoComVinculosPorId(id);
        }

        public void AtualizarValoresDaAcao(AcaoInputModel objAcao)
        {
            Acao dbEntityAnterior = GetById(objAcao.Id);
            _acaoRepository.AtualizarValoresDaAcao(objAcao);
            Acao dbEntityAlterado = GetById(objAcao.Id);
            _entityTrackService.RegisterUpdate(dbEntityAnterior, dbEntityAlterado);

            AtualizarUsuarios(objAcao);
        }

        #region Alterar Status de Ação para atrasado
        private void AtualizarStatusDaAcaoParaAtrasado(List<AcaoViewModel> acoes)
        {
            if (acoes.Count == 0)
            {
                return;
            }

            acoes.ForEach(acao =>
            {
                Acao dbEntityAnterior = GetById(acao.Id);

                _acaoRepository.AtualizarStatusDaAcaoParaAtrasado(acao);

                Acao dbEntityAlterado = GetById(acao.Id);
                _entityTrackService.RegisterUpdate(dbEntityAnterior, dbEntityAlterado);

                EnviarEmail(null, acao.Id);
            });
        }

        private List<AcaoViewModel> ObterAcoesAtrasadas()
        {
            return _acaoRepository.ObterAcoesAtrasadas();
        }

        public void AplicarRegraDeAcaoAtrasadaFacade()
        {
            var acoes = this.ObterAcoesAtrasadas();
            this.AtualizarStatusDaAcaoParaAtrasado(acoes);
        }
        #endregion

        public void AlterarStatusComBaseNoAcompanhamento(int id, AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            Acao dbEntityAnterior = GetById(id);

            _acaoRepository.AlterarStatusComBaseNoAcompanhamento(id, objAcompanhamentoAcao);

            Acao dbEntityAlterado = GetById(id);
            _entityTrackService.RegisterUpdate(dbEntityAnterior, dbEntityAlterado);
        }
    }
}

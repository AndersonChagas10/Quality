using Conformity.Application.Core.Log;
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

namespace Conformity.Application.Core.PlanoDeAcao
{
    public class AcaoService : BaseServiceWithLog<Acao>
    {
        private readonly AcaoRepository _acaoRepository;

        public AcaoService(IRepositoryNoLazyLoad<Acao> repository
            , EntityTrackService historicoAlteracaoService,
            AcaoRepository acaoRepository)
            : base(repository
                  , historicoAlteracaoService)
        {
            _acaoRepository = acaoRepository;
        }
        public IEnumerable<AcaoViewModel> ObterAcaoPorFiltro(FiltroListagemDeAcaoDoWorkflow form)
        {
            return _acaoRepository.ObterAcao(form);
        }

        public void EnviarEmail(AcaoInputModel acao)
        {
            //1 Pendente - nao envia email
            //2 Em andamento - cenario 1 e 2
            //3 Concluída - cenario 3 e 4
            //4 Atrasada  - cenario 5 e 6
            //5 Cancelada - cenario 7 e 8

            var acaoCompleta = _acaoRepository.GetById(acao.Id);

            if (acao.Status == EAcaoStatus.Em_Andamento)
            {
                var emailResponsavel = new MontaEmail(new EmailCreateAcaoResponsavel(acaoCompleta));
                EnviarEmailAcao(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoNotificados(acaoCompleta));
                EnviarEmailAcao(emailNotificados);
            }

            if (acao.Status == EAcaoStatus.Concluída)
            {
                var emailResponsavel = new MontaEmail(new EmailCreateAcaoVerEAgirResponsavel(acaoCompleta));
                EnviarEmailAcao(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoVerEAgirNotificados(acaoCompleta));
                EnviarEmailAcao(emailNotificados);

            }

            if (acao.Status == EAcaoStatus.Atrasada)
            {
                var emailResponsavel = new MontaEmail(new EmailAcaoVencidaResponsavel(acaoCompleta));
                EnviarEmailAcao(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailAcaoVencidaNotificados(acaoCompleta));
                EnviarEmailAcao(emailNotificados);
            }

            if (acao.Status == EAcaoStatus.Cancelada)
            {
                var emailResponsavel = new MontaEmail(new EmailAcaoStatusCanceladoResponsavel(acaoCompleta));
                EnviarEmailAcao(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailAcaoStatusCanceladoNotificados(acaoCompleta));
                EnviarEmailAcao(emailNotificados);
            }
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
            Task.Run(() =>
            Conformity.Infra.CrossCutting.MailHelper.SendMail(
                (DicionarioEstatico.DicionarioEstaticoHelpers.emailFrom as string),
                (DicionarioEstatico.DicionarioEstaticoHelpers.emailPass as string),
                (DicionarioEstatico.DicionarioEstaticoHelpers.emailSmtp as string),
                int.Parse(DicionarioEstatico.DicionarioEstaticoHelpers.emailPort as string),
                bool.Parse(DicionarioEstatico.DicionarioEstaticoHelpers.emailSSL as string),
                string.Join(",", email.GetEmail().To),
                email.GetEmail().Subject,
                email.GetEmail().Body,
                DicionarioEstatico.DicionarioEstaticoHelpers.systemName));
        }

        public AcaoFormViewModel ObterAcaoComVinculosPorId(int id)
        {
            return _acaoRepository.ObterAcaoComVinculosPorId(id);
        }

        public void AtualizarValoresDaAcao(AcaoInputModel objAcao)
        {
            _acaoRepository.AtualizarValoresDaAcao(objAcao);
        }

        public void AlterarStatusComBaseNoAcompanhamento(int id, AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            _acaoRepository.AlterarStatusComBaseNoAcompanhamento(id, objAcompanhamentoAcao);
        }
    }
}

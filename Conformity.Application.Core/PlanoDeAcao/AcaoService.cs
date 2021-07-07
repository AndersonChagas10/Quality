using Conformity.Application.Core.Log;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.DTOs.Filtros;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core.Repository.PlanoDeAcao;
using System.Collections.Generic;
using System.Linq;
using static Conformity.Domain.Core.Enums.PlanoDeAcao.Enums;

namespace Conformity.Application.Core.PlanoDeAcao
{
    public class AcaoService : BaseServiceWithLog<EvidenciaConcluida>
    {
        private readonly AcaoRepository _acaoRepository;

        public AcaoService(IRepositoryNoLazyLoad<EvidenciaConcluida> repository
            , EntityTrackService historicoAlteracaoService,
            AcaoRepository acaoRepository)
            : base(repository
                  , historicoAlteracaoService)
        {
            _acaoRepository = acaoRepository;
        }
        public IEnumerable<AcaoViewModel> ObterAcaoPorFiltro(FiltroListagemDeAcaoDoWorkflow form, UserSgq usuarioLogado)
        {
            return _acaoRepository.ObterAcao(form, usuarioLogado);
        }

        public void EnviarEmail(AcaoInputModel acao)
        {
            //1 Pendente - nao envia email
            //2 Em andamento - cenario 1 e 2
            //3 Concluída - cenario 3 e 4
            //4 Atrasada  - cenario 5 e 6
            //5 Cancelada - cenario 7 e 8

            var acaoCompleta = new AcaoBusiness().GetBy(acao.Id);

            if (acao.Status == EAcaoStatus.Em_Andamento)
            {
                var emailResponsavel = new MontaEmail(new EmailCreateAcaoResponsavel(acaoCompleta));
                EmailAcaoService.Send(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoNotificados(acaoCompleta));
                EmailAcaoService.Send(emailNotificados);
            }

            if (acao.Status == EAcaoStatus.Concluída)
            {
                var emailResponsavel = new MontaEmail(new EmailCreateAcaoVerEAgirResponsavel(acaoCompleta));
                EmailAcaoService.Send(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoVerEAgirNotificados(acaoCompleta));
                EmailAcaoService.Send(emailNotificados);

            }

            if (acao.Status == EAcaoStatus.Atrasada)
            {
                var emailResponsavel = new MontaEmail(new EmailAcaoVencidaResponsavel(acaoCompleta));
                EmailAcaoService.Send(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailAcaoVencidaNotificados(acaoCompleta));
                EmailAcaoService.Send(emailNotificados);
            }

            if (acao.Status == EAcaoStatus.Cancelada)
            {
                var emailResponsavel = new MontaEmail(new EmailAcaoStatusCanceladoResponsavel(acaoCompleta));
                EmailAcaoService.Send(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailAcaoStatusCanceladoNotificados(acaoCompleta));
                EmailAcaoService.Send(emailNotificados);
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
    }
}

using Data.PlanoDeAcao.Interfaces;
using Data.PlanoDeAcao.Repositorio;
using Dominio;
using Dominio.AcaoRH;
using Dominio.AcaoRH.Email;
using DTO;
using DTO.PlanoDeAcao;
using SgqServiceBusiness.Controllers.RH;
using System.Collections.Generic;
using System.Linq;

namespace Services.PlanoDeAcao
{
    public class AcaoService : Interfaces.IAcaoService
    {
        private readonly IAcaoRepository _acaoRepository;

        public AcaoService(IAcaoRepository acaoRepository)
        {
            _acaoRepository = acaoRepository;
        }

        public IEnumerable<AcaoViewModel> ObterAcaoPorFiltro(DataCarrierFormularioNew form, UserSgq usuarioLogado)
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

using Conformity.Application.Core.Log;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.CrossCutting;
using Conformity.Infra.Data.Core.Repository.PlanoDeAcao;
using System;
using System.Linq;

namespace Conformity.Application.Core.PlanoDeAcao
{
    public class AcompanhamentoAcaoService : BaseServiceWithLog<AcompanhamentoAcao>
    {
        private readonly ApplicationConfig _applicationConfig;

        public AcompanhamentoAcaoService(IRepositoryNoLazyLoad<AcompanhamentoAcao> repository
            , ApplicationConfig applicationConfig
            , EntityTrackService historicoAlteracaoService)
            : base(repository
                  , historicoAlteracaoService)
        {
            _applicationConfig = applicationConfig;
        }

        public AcaoViewModel SalvarAcompanhamentoComNotificaveis(int id, AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            try
            {
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
                    UserSgq_Id = _applicationConfig.Authenticated_Id
                };
                _repository.Add(acompanhamento);
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}

﻿using Conformity.Application.Core.Log;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core.Repository.PlanoDeAcao;
using System;
using System.Linq;

namespace Conformity.Application.Core.PlanoDeAcao
{
    public class AcompanhamentoAcaoService : BaseServiceWithLog<AcompanhamentoAcao>
    {
        private readonly AcompanhamentoAcaoRepository _acompanhamentoAcaoRepository;

        public AcompanhamentoAcaoService(IRepositoryNoLazyLoad<AcompanhamentoAcao> repository
            , EntityTrackService historicoAlteracaoService,
            AcompanhamentoAcaoRepository acompanhamentoAcaoRepository)
            : base(repository
                  , historicoAlteracaoService)
        {
            _acompanhamentoAcaoRepository = acompanhamentoAcaoRepository;
        }

        public AcaoViewModel SalvarAcompanhamentoComNotificaveis(int id, AcompanhamentoAcaoInputModel objAcompanhamentoAcao, UserSgq usuarioLogado)
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
                    UserSgq_Id = usuarioLogado.Id
                };
                _acompanhamentoAcaoRepository.SalvarAcompanhamentoAcao(acompanhamento);
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}

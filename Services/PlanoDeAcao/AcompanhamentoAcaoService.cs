using Data.PlanoDeAcao.Interfaces;
using Data.PlanoDeAcao.Repositorio;
using Dominio;
using Dominio.AcaoRH;
using DTO.PlanoDeAcao;
using System;
using System.Linq;

namespace Services.PlanoDeAcao
{
    public class AcompanhamentoAcaoService : Interfaces.IAcompanhamentoAcaoService
    {
        private readonly IAcompanhamentoAcaoRepository _acompanhamentoRepository;

        public AcompanhamentoAcaoService(IAcompanhamentoAcaoRepository acompanhamentoRepository)
        {
            _acompanhamentoRepository = acompanhamentoRepository;
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
                _acompanhamentoRepository.SalvarAcompanhamentoAcao(acompanhamento);
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}

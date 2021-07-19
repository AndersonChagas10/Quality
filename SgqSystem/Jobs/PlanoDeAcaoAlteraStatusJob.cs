using ADOFactory;
using Conformity.Application.Core.PlanoDeAcao;
using Conformity.Domain.Core.DTOs;
using Ninject;
using Quartz;
using SgqSystem.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SgqSystem.Jobs
{
    public class PlanoDeAcaoAlteraStatusJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ExecutarAlteracoesDeAcoes(null);
        }

        public static void ExecutarAlteracoesDeAcoes(object stateInfo)
        {
            Task.Run(() => VerificarEAtualizarAcoesAtrasadas());
        }

        private static void VerificarEAtualizarAcoesAtrasadas()
        {
            while (true)
            {
                int intervaloDeTempo = 10000;

                var acoes = ObterAcoesAtrasadas();

                AtualizarAcoesAtrasadas(acoes);

                Thread.Sleep(intervaloDeTempo);
            }
        }

        private static List<AcaoViewModel> ObterAcoesAtrasadas()
        {
            var query = $@"SELECT * FROM Pa.Acao 
                            WHERE Status = 2
                            AND DataConclusao < '{DateTime.Now.AddSeconds(-20):yyyy-MM-dd HH:mm}'";

            var acoes = new List<AcaoViewModel>();

            using (Factory factory = new Factory("DefaultConnection"))
            {
                try
                {
                    acoes = factory.SearchQuery<AcaoViewModel>(query).ToList();
                }
                catch (Exception ex)
                {
                    LogSystem.LogErrorBusiness.Register(ex, new { query });
                }
                return acoes;
            }
        }

        private static void AtualizarAcoesAtrasadas(List<AcaoViewModel> acoes)
        {
            if (acoes.Count == 0)
            {
                return;
            }

            acoes.ForEach(acao =>
            {
                var acaoService = NinjectWebCommon.Kernel.Get<AcaoService>();
                acaoService.AtualizarValoresDaAcao(acao);
            });
        }
    }
}
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
    public class PlanoDeAcaoAlterarStatusJob : IJob
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
                var acaoService = NinjectWebCommon.Kernel.Get<AcaoService>();

                acaoService.AplicarRegraDeAcaoAtrasadaFacade();

                Thread.Sleep(10000);
            }
        }
    }
}
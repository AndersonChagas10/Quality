using Conformity.Application.Core;
using Conformity.Application.Core.Log;
using Conformity.Application.Core.Parametrizacao;
using Conformity.Application.Core.PlanoDeAcao;
using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core;
using Ninject.Modules;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Infra.IoC
{
    public class ServiceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            LogService();
            ParametrizacaoService();
            PlanoDeAcaoService();
        }

        public void LogService()
        {
            Bind(typeof(EntityTrackService)).To(typeof(EntityTrackService));
        }

        public void ParametrizacaoService()
        {
            Bind(typeof(ParCompanyService)).To(typeof(ParCompanyService));
        }

        public void PlanoDeAcaoService()
        {
            Bind(typeof(AcaoService)).To(typeof(AcaoService));
            Bind(typeof(AcompanhamentoAcaoService)).To(typeof(AcompanhamentoAcaoService));
            Bind(typeof(EvidenciaConcluidaService)).To(typeof(EvidenciaConcluidaService));
            Bind(typeof(EvidenciaNaoConformeService)).To(typeof(EvidenciaNaoConformeService));
        }
    }
}

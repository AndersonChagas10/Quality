using Conformity.Application.Core.Core;
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

namespace Conformity.Infra.IoC.Core
{
    public class ServiceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            CustomService();
        }

        public void CustomService()
        {
            Bind(typeof(HistoricoAlteracaoService)).To(typeof(HistoricoAlteracaoService));
            Bind(typeof(ParCompanyService)).To(typeof(ParCompanyService));
        }
    }
}

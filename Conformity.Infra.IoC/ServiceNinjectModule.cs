using Conformity.Application.Core;
using Conformity.Application.Core.Log;
using Conformity.Application.Core.Parametrizacao;
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
        }

        public void LogService()
        {
            Bind(typeof(EntityTrackService)).To(typeof(EntityTrackService));
        }

        public void ParametrizacaoService()
        {
            Bind(typeof(ParCompanyService)).To(typeof(ParCompanyService));
        }
    }
}

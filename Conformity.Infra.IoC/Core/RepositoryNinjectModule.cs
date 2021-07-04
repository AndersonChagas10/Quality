using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core;
using Conformity.Infra.Data.Core.Core;
using Ninject.Modules;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conformity.Infra.IoC.Core
{
    public class RepositoryNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IRepository<>)).To(typeof(RepositoryBase<>));
            Bind(typeof(IRepositoryNoLazyLoad<>)).To(typeof(RepositoryBaseNoLazyLoad<>));
            CustomRepository();
        }

        public void CustomRepository()
        {
            Bind(typeof(HistoricoAlteracaoRepository)).To(typeof(HistoricoAlteracaoRepository));
        }
    }
}

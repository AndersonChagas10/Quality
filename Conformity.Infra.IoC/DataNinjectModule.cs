using Conformity.Application.Util;
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
    public class DataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<EntityContext>().ToSelf().InRequestScope();
        }
    }
}

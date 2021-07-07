using Conformity.Infra.Data.Core;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Conformity.Infra.IoC
{
    public class DataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<EntityContext>().ToSelf().InRequestScope();
            Bind<ADOContext>().ToSelf().InRequestScope();
        }
    }
}

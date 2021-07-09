using Conformity.Infra.Data.Core;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Conformity.Infra.IoC
{
    public class DataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            EntityContext();
            Bind<ADOContext>().ToSelf().InRequestScope();
        }

        public void EntityContext()
        {
            Bind<PlanoDeAcaoEntityContext>().ToSelf().InRequestScope();
            Bind<LogEntityContext>().ToSelf().InRequestScope();
            Bind<ParametrizacaoEntityContext>().ToSelf().InRequestScope();
        }
    }
}

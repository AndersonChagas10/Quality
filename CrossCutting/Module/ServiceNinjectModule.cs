using Dominio.Interfaces.Services;
using Dominio.Services;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class ServiceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IServiceBase<>)).To(typeof(ServiceBase<>));
            Bind<IUserService>().To<UserService>();
            Bind<IResultOldService>().To<ResultOldService>();
            Bind<IRelatorioBetaService>().To<RelatorioBetaService>();
        }
    }
}

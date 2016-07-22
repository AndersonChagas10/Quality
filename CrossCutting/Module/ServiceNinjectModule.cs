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
            Bind<IBetaService>().To<BetaService>();
            Bind<ISyncDomain>().To<SyncService>();
        }
    }
}

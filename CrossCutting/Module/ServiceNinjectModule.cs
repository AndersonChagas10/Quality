using Dominio.Interfaces.Services;
using Dominio.Services;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class ServiceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBaseDomain<>)).To(typeof(BaseDomain<>));
            Bind<IUserDomain>().To<UserDomain>();
            //Bind<IBetaService>().To<BetaService>();
            Bind<ICorrectiveActionDomain>().To<CorrectiveActionDomain>();
            //Bind<ISyncDomain>().To<SyncDomain>();
            //Bind<IRelatorioColetaDomain>().To<RelatorioColetaDomain>();
            //Bind<IColetaDomain>().To<ColetaDomain>();
            Bind<ISaveConsolidateDataCollectionDomain>().To<SaveConsolidateDataCollectionDomain>();
        }
    }
}

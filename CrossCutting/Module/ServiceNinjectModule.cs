using Dominio.Interfaces.Services;
using Dominio.Services;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class ServiceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBaseDomain<,>)).To(typeof(BaseDomain<,>));
            Bind<IUserDomain>().To<UserDomain>();
            Bind<ICorrectiveActionDomain>().To<CorrectiveActionDomain>();
            Bind<IRelatorioColetaDomain>().To<RelatorioColetaDomain>();
            Bind<ISaveConsolidateDataCollectionDomain>().To<SaveConsolidateDataCollectionDomain>();
            Bind<IGetConsolidateDataCollectionDomain>().To<GetConsolidateDataCollectionDomain>();
            Bind<IParamsDomain>().To<ParamsDomain>();
        }
    }
}

using Dominio.Interfaces.Services;
using Dominio.Services;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class DomainNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBaseDomain<,>)).To(typeof(BaseDomain<,>));
            Bind<IUserDomain>().To<UserDomain>();
            Bind<IRelatorioColetaDomain>().To<RelatorioColetaDomain>();
            Bind<IExampleDomain>().To<ExampleDomain>();
            Bind<IParamsDomain>().To<ParamsDomain>();
            Bind<ICompanyDomain>().To<CompanyDomain>();
            Bind<IDefectDomain>().To<DefectDomain>();

            //Bind<ICorrectiveActionDomain>().To<CorrectiveActionDomain>();
            //Bind<ISaveConsolidateDataCollectionDomain>().To<SaveConsolidateDataCollectionDomain>();
            //Bind<IGetConsolidateDataCollectionDomain>().To<GetConsolidateDataCollectionDomain>();
        }
    }
}

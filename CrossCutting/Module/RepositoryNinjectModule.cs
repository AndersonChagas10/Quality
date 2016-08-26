using Data.Repositories;
using Dominio.Interfaces.Repositories;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class RepositoryNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBaseRepository<>)).To(typeof(RepositoryBase<>));
            Bind(typeof(IGetDataResultRepository<>)).To(typeof(GetDataResultRepository<>));
            //Bind(typeof(ISyncRepository<>)).To(typeof(SyncRepository<>));
            Bind<IUserRepository>().To<UserRepository>();
            //Bind<IBetaRepository>().To<BetaRepository>();
            Bind<ICorrectiveActionRepository>().To<CorrectiveActionRepository>();
            //Bind<IConsolidationLevel01Repository>().To<ConsolidationLevel01Repository>();
            //Bind<IRelatorioColetaRepository>().To<RelatorioColetaRepository>();
            //Bind<IColetaRepository>().To<ColetaRepository>();
        }
    }
}

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
            Bind(typeof(ISyncRepository<>)).To(typeof(SyncRepository<>));
            Bind<IUserRepository>().To<UserRepository>();
            //Bind<IBetaRepository>().To<BetaRepository>();
            Bind<ICorrectiveActionRepository>().To<CorrectiveActionRepository>();
            Bind<IRelatorioColetaRepository>().To<RelatorioColetaRepository>();
            Bind<IColetaRepository>().To<ColetaRepository>();
        }
    }
}

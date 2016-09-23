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
            Bind(typeof(IRelatorioColetaRepository<>)).To(typeof(RelatorioColetaRepository<>));
            Bind<IUserRepository>().To<UserRepository>();
            Bind<ICollectionLevel02Repo>().To<CollectionLevel02Repo>();
            Bind<ICorrectiveActionRepository>().To<CorrectiveActionRepository>();
            //Bind<IRelatorioColetaRepository>().To<RelatorioColetaRepository>();
            //Bind(typeof(ISyncRepository<>)).To(typeof(SyncRepository<>));
            //Bind<IColetaRepository>().To<ColetaRepository>();
        }
    }
}

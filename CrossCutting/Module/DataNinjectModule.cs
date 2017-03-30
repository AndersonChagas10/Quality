using Data.Repositories;
using Dominio.Interfaces.Repositories;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class DataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBaseRepository<>)).To(typeof(RepositoryBase<>));
            Bind(typeof(IBaseRepositoryNoLazyLoad<>)).To(typeof(RepositoryBaseNoLazyLoad<>));
            Bind(typeof(IRelatorioColetaRepository<>)).To(typeof(RelatorioColetaRepository<>));
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IParamsRepository>().To<ParamsRepository>();
            Bind<IParLevel3Repository>().To<ParLevel3Repository>();

            //Bind(typeof(IGetDataResultRepository<>)).To(typeof(GetDataResultRepository<>));
            //Bind<ICollectionLevel02Repo>().To<CollectionLevel02Repo>();
            //Bind<ICorrectiveActionRepository>().To<CorrectiveActionRepository>();
            //Bind<ISaveCollectionRepo>().To<SaveCollectionRepo>();
            //Bind(typeof(ISyncRepository<>)).To(typeof(SyncRepository<>));
            //Bind<IColetaRepository>().To<ColetaRepository>();
        }
    }
}

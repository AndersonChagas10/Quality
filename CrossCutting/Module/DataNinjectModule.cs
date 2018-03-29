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
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IParamsRepository>().To<ParamsRepository>();
            Bind<IParLevel3Repository>().To<ParLevel3Repository>();
        }
    }
}

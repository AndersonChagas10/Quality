using Data.Repositories;
using Dominio.Interfaces.Repositories;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class RepositoryNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IRepositoryBase<>)).To(typeof(RepositoryBase<>));
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IBetaRepository>().To<BetaRepository>();
        }
    }
}

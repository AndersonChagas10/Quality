using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core;
using Conformity.Infra.Data.Core.Repository;
using Conformity.Infra.Data.Core.Repository.Log;
using Ninject.Modules;

namespace Conformity.Infra.IoC.Core
{
    public class RepositoryNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IRepository<>)).To(typeof(RepositoryBase<>));
            Bind(typeof(IRepositoryNoLazyLoad<>)).To(typeof(RepositoryBaseNoLazyLoad<>));
            LogRepository();
        }

        public void LogRepository()
        {
            Bind(typeof(EntityTrackRepository)).To(typeof(EntityTrackRepository));
        }
    }
}

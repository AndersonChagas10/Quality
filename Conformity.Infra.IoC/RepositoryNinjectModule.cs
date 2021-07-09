using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.Data.Core;
using Conformity.Infra.Data.Core.Repository;
using Conformity.Infra.Data.Core.Repository.Log;
using Conformity.Infra.Data.Core.Repository.PlanoDeAcao;
using Ninject.Modules;

namespace Conformity.Infra.IoC
{
    public class RepositoryNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IRepository<>)).To(typeof(RepositoryBase<>));
            Bind(typeof(IRepositoryNoLazyLoad<>)).To(typeof(RepositoryBaseNoLazyLoad<>));
            LogRepository();
            ParametrizacaoRepository();
            PlanoDeAcaoRepository();
        }

        public void LogRepository()
        {
            
            Bind(typeof(ILogRepository<>)).To(typeof(LogRepositoryBase<>));
            Bind(typeof(ILogRepositoryNoLazyLoad<>)).To(typeof(LogRepositoryBaseNoLazyLoad<>));
            Bind(typeof(EntityTrackRepository)).To(typeof(EntityTrackRepository));
        }

        public void PlanoDeAcaoRepository()
        {

            Bind(typeof(IPlanoDeAcaoRepository<>)).To(typeof(PlanoDeAcaoRepositoryBase<>));
            Bind(typeof(IPlanoDeAcaoRepositoryNoLazyLoad<>)).To(typeof(PlanoDeAcaoRepositoryBaseNoLazyLoad<>));
            Bind<AcaoRepository>().To<AcaoRepository>();
            Bind<EvidenciaNaoConformeRepository>().To<EvidenciaNaoConformeRepository>();
            Bind<EvidenciaConcluidaRepository>().To<EvidenciaConcluidaRepository>();
        }

        public void ParametrizacaoRepository()
        {
            Bind(typeof(IParametrizacaoRepository<>)).To(typeof(ParametrizacaoRepositoryBase<>));
            Bind(typeof(IParametrizacaoRepositoryNoLazyLoad<>)).To(typeof(ParametrizacaoRepositoryBaseNoLazyLoad<>));
        }
    }
}

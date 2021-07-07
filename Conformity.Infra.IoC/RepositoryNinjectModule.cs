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
        }

        public void LogRepository()
        {
            Bind(typeof(EntityTrackRepository)).To(typeof(EntityTrackRepository));
        }

        public void PlanoDeAcaoRepository()
        {
            Bind<AcaoRepository>().To<AcaoRepository>();
            Bind<AcompanhamentoAcaoRepository>().To<AcompanhamentoAcaoRepository>();
            Bind<EvidenciaNaoConformeRepository>().To<EvidenciaNaoConformeRepository>();
            Bind<EvidenciaConcluidaRepository>().To<EvidenciaConcluidaRepository>();
        }
    }
}

using Application.AppServiceClass;
using Application.Interface;
using Ninject.Modules;

namespace CrossCutting.IOC.Module
{
    public class ApplicationNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IBaseApp<>)).To(typeof(AppServiceBase<>));
            Bind<IUserApp>().To<UserApp>();
            //Bind<IBetaAppService>().To<BetaAppService>();
            Bind<ICorrectiveActionAppService>().To<CorrectiveActionAppService>();
            Bind<IRelatorioColetaApp>().To<RelatorioColetaApp>();
            Bind<ISyncApp>().To<SyncApp>();
            Bind<IColetaApp>().To<ColetaApp>();
        }
    }
}